import base64
import hmac
import json
import logging
import os
import time
import uuid
from datetime import datetime, timedelta, timezone
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import urlsplit
from urllib.error import HTTPError, URLError
from urllib.request import Request, urlopen

try:
    import pyodbc
except ImportError:
    pyodbc = None


HTTP_HOST = os.environ.get("SMARTLOCK_HTTP_HOST", "0.0.0.0")
HTTP_PORT = int(os.environ.get("SMARTLOCK_HTTP_PORT", "8080"))
HTTP_USERNAME = os.environ.get("SMARTLOCK_HTTP_USERNAME", "")
HTTP_PASSWORD = os.environ.get("SMARTLOCK_HTTP_PASSWORD", "")

SQL_SERVER = os.environ.get(
    "SMARTLOCK_SQL_SERVER", "SMARTLOCK_BD.mssql.somee.com"
)
SQL_DATABASE = os.environ.get("SMARTLOCK_SQL_DATABASE", "SMARTLOCK_BD")
SQL_USERNAME = os.environ.get("SMARTLOCK_SQL_USERNAME", "")
SQL_PASSWORD = os.environ.get("SMARTLOCK_SQL_PASSWORD", "")
SQL_DRIVER = os.environ.get("SMARTLOCK_SQL_DRIVER", "ODBC Driver 17 for SQL Server")
SQL_PORT = int(os.environ.get("SMARTLOCK_SQL_PORT", "1433"))
SQL_ENCRYPT = os.environ.get("SMARTLOCK_SQL_ENCRYPT", "yes")
SQL_TRUST_CERTIFICATE = os.environ.get(
    "SMARTLOCK_SQL_TRUST_SERVER_CERTIFICATE", "yes"
)

RFID_API_BASE_URL = os.environ.get(
    "SMARTLOCK_RFID_API_BASE_URL", "http://127.0.0.1:5000"
).rstrip("/")
RFID_DEVICE_API_KEY = os.environ.get("SMARTLOCK_RFID_DEVICE_API_KEY", "")
RFID_EVENT_MARKER = "SMARTLOCK:RFID:PERMANENT_ACCESS:v1"
RFID_ACCESS_METHOD = "RFID"

CACHE_TTL_SECONDS = 60 * 60
MAX_REQUEST_BYTES = 128 * 1024
MAX_SYNC_RECORDS = 128
OFFLINE_ACCESS_METHOD = "Solo QR offline"

QR_CACHE_QUERY = """
SELECT
    de.id_detalle_evento,
    de.codigo_qr,
    de.id_usuario,
    u.estado_usuario,
    CAST(0 AS bit) AS pin_configurado,
    de.id_evento,
    e.nombre_evento,
    e.estado_evento,
    e.fechaprogramada_evento,
    e.fechalimite_evento,
    de.qr_usado,
    CASE
        WHEN e.fechaprogramada_evento IS NULL
          OR e.fechaprogramada_evento <= ahora.fecha_actual THEN 0
        ELSE DATEDIFF(SECOND, ahora.fecha_actual, e.fechaprogramada_evento)
    END AS disponible_en_segundos,
    CASE
        WHEN e.fechalimite_evento IS NULL THEN 3600
        WHEN DATEDIFF(SECOND, ahora.fecha_actual, e.fechalimite_evento) > 3600 THEN 3600
        ELSE DATEDIFF(SECOND, ahora.fecha_actual, e.fechalimite_evento)
    END AS valido_por_segundos
FROM dbo.detalle_eventos AS de
INNER JOIN dbo.eventos AS e ON e.id_evento = de.id_evento
INNER JOIN dbo.usuarios AS u ON u.id_usuario = de.id_usuario
CROSS APPLY (SELECT CAST(? AS datetime) AS fecha_actual) AS ahora
WHERE de.codigo_qr IS NOT NULL
  AND de.qr_usado = 0
  AND e.estado_evento = 1
  AND u.estado_usuario = 1
  AND (
      e.fechaprogramada_evento IS NULL
      OR e.fechaprogramada_evento <= DATEADD(SECOND, 3600, ahora.fecha_actual)
  )
  AND (
      e.fechalimite_evento IS NULL
      OR e.fechalimite_evento > ahora.fecha_actual
  )
ORDER BY de.id_detalle_evento;
"""

OFFLINE_ACCESS_UPDATE = """
UPDATE de
SET de.qr_usado = 1,
    de.acceso_detalle_evento = ?,
    de.metodo_acceso = 'Solo QR offline'
OUTPUT inserted.id_detalle_evento
FROM dbo.detalle_eventos AS de
INNER JOIN dbo.eventos AS e ON e.id_evento = de.id_evento
WHERE de.id_detalle_evento = ?
  AND de.codigo_qr = ?
  AND de.id_usuario = ?
  AND de.id_evento = ?
  AND de.qr_usado = 0
  AND (
      e.fechaprogramada_evento IS NULL
      OR e.fechaprogramada_evento <= ?
  )
  AND (
      e.fechalimite_evento IS NULL
      OR e.fechalimite_evento > ?
  );
"""

OFFLINE_ACCESS_LOOKUP = """
SELECT qr_usado, acceso_detalle_evento, metodo_acceso
FROM dbo.detalle_eventos
WHERE id_detalle_evento = ?
  AND codigo_qr = ?;
"""

RFID_OFFLINE_INSERT = """
INSERT INTO dbo.detalle_eventos
    (id_usuario, id_evento, acceso_detalle_evento, metodo_acceso)
OUTPUT INSERTED.id_detalle_evento
VALUES (?, ?, ?, 'RFID');
"""

RFID_SYNC_LOOKUP = """
SELECT id_detalle_evento
FROM dbo.smartlock_rfid_sync WITH (UPDLOCK, HOLDLOCK)
WHERE registro_id = ?;
"""

if pyodbc is None:
    ODBC_ERROR = RuntimeError
else:
    ODBC_ERROR = pyodbc.Error


class DatabaseUnavailable(RuntimeError):
    pass


def _odbc_value(value):
    return "{" + str(value).replace("}", "}}") + "}"


def open_database():
    if pyodbc is None:
        raise DatabaseUnavailable("pyodbc no está instalado.")

    missing = [
        name
        for name, value in (
            ("SMARTLOCK_SQL_USERNAME", SQL_USERNAME),
            ("SMARTLOCK_SQL_PASSWORD", SQL_PASSWORD),
        )
        if not value
    ]
    if missing:
        raise DatabaseUnavailable(
            "Faltan variables de entorno SQL: " + ", ".join(missing)
        )

    connection_string = ";".join(
        (
            f"DRIVER={_odbc_value(SQL_DRIVER)}",
            f"SERVER={_odbc_value(f'{SQL_SERVER},{SQL_PORT}')}",
            f"DATABASE={_odbc_value(SQL_DATABASE)}",
            f"UID={_odbc_value(SQL_USERNAME)}",
            f"PWD={_odbc_value(SQL_PASSWORD)}",
            f"Encrypt={SQL_ENCRYPT}",
            f"TrustServerCertificate={SQL_TRUST_CERTIFICATE}",
            "Connection Timeout=5",
        )
    )
    try:
        return pyodbc.connect(connection_string, timeout=5, autocommit=False)
    except ODBC_ERROR as exc:
        raise DatabaseUnavailable("No se pudo conectar con SQL Server.") from exc


def _sql_wall_epoch(value):
    if value is None:
        return None
    if value.tzinfo is not None:
        value = value.replace(tzinfo=None)
    return int(value.replace(tzinfo=timezone.utc).timestamp())


def _iso_sql_datetime(value):
    return value.isoformat(sep=" ", timespec="seconds") if value else None


def _serialize_cache_row(row):
    fecha_inicio = row.fechaprogramada_evento
    fecha_limite = row.fechalimite_evento
    return {
        "idDetalleEvento": int(row.id_detalle_evento),
        "codigoQr": str(row.codigo_qr),
        "idUsuario": int(row.id_usuario),
        "estadoUsuario": int(row.estado_usuario) == 1,
        "pinConfigurado": bool(row.pin_configurado),
        "idEvento": int(row.id_evento),
        "nombreEvento": str(row.nombre_evento),
        "estadoEvento": int(row.estado_evento) == 1,
        "fechaProgramadaSql": _iso_sql_datetime(fecha_inicio),
        "fechaProgramadaEpoch": _sql_wall_epoch(fecha_inicio),
        "fechaLimiteSql": _iso_sql_datetime(fecha_limite),
        "fechaLimiteEpoch": _sql_wall_epoch(fecha_limite),
        "qrUsado": bool(row.qr_usado),
        "disponibleEnSegundos": max(0, int(row.disponible_en_segundos)),
        "validoPorSegundos": max(0, int(row.valido_por_segundos)),
    }


def get_authorized_qrs():
    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute("SELECT GETDATE() AS fecha_actual;")
        fecha_actual = cursor.fetchone().fecha_actual
        if fecha_actual is None:
            raise DatabaseUnavailable("SQL Server no devolvió la hora actual.")

        cursor.execute(QR_CACHE_QUERY, fecha_actual)
        records = [_serialize_cache_row(row) for row in cursor.fetchall()]
        connection.commit()
        sql_epoch = _sql_wall_epoch(fecha_actual)
        utc_epoch = int(time.time())
        return {
            "cacheTtlSeconds": CACHE_TTL_SECONDS,
            "snapshotSqlEpoch": sql_epoch,
            "serverUtcEpoch": utc_epoch,
            "sqlUtcOffsetSeconds": sql_epoch - utc_epoch,
            "qrAutorizados": records,
        }
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló la consulta de QR autorizados.") from exc
    finally:
        connection.close()


def _parse_access_datetime(value):
    if not isinstance(value, int) or value <= 0:
        raise ValueError("fechaAccesoSqlEpoch debe ser un entero positivo.")
    try:
        return datetime.fromtimestamp(value, timezone.utc).replace(tzinfo=None)
    except (OverflowError, OSError, ValueError) as exc:
        raise ValueError("fechaAccesoSqlEpoch está fuera de rango.") from exc


def _access_already_synced(connection, record):
    cursor = connection.cursor()
    cursor.execute(
        OFFLINE_ACCESS_LOOKUP,
        record["idDetalleEvento"],
        record["codigoQr"],
    )
    row = cursor.fetchone()
    if row is None or not bool(row.qr_usado):
        return False
    if row.metodo_acceso != OFFLINE_ACCESS_METHOD or row.acceso_detalle_evento is None:
        return False

    requested_at = _parse_access_datetime(record["fechaAccesoSqlEpoch"])
    stored_at = row.acceso_detalle_evento
    return abs((stored_at - requested_at).total_seconds()) <= CACHE_TTL_SECONDS


def sync_access(record):
    fecha_acceso = _parse_access_datetime(record["fechaAccesoSqlEpoch"])
    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute(
            OFFLINE_ACCESS_UPDATE,
            fecha_acceso,
            record["idDetalleEvento"],
            record["codigoQr"],
            record["idUsuario"],
            record["idEvento"],
            fecha_acceso,
            fecha_acceso,
        )
        updated = cursor.fetchone()
        if updated is not None:
            connection.commit()
            return True, None

        if _access_already_synced(connection, record):
            connection.commit()
            return True, None

        connection.rollback()
        return False, "El QR no está disponible o ya fue consumido por otro flujo."
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló la sincronización de un acceso.") from exc
    finally:
        connection.close()


def sync_rfid_access(record):
    fecha_acceso = _parse_access_datetime(record["fechaAccesoSqlEpoch"])
    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute(RFID_SYNC_LOOKUP, record["registroId"])
        existing = cursor.fetchone()
        if existing is not None:
            connection.commit()
            return True, None

        cursor.execute(
            """
            SELECT TOP (1) id_evento
            FROM dbo.eventos WITH (UPDLOCK, HOLDLOCK)
            WHERE descripcion_evento = ?;
            """,
            RFID_EVENT_MARKER,
        )
        event_row = cursor.fetchone()
        if event_row is None:
            connection.rollback()
            return False, "No existe el evento permanente RFID."

        cursor.execute(
            "SELECT 1 FROM dbo.usuarios WHERE id_usuario = ?;",
            record["idUsuario"],
        )
        if cursor.fetchone() is None:
            connection.rollback()
            return False, "El usuario del acceso offline ya no existe."

        cursor.execute(
            RFID_OFFLINE_INSERT,
            record["idUsuario"],
            int(event_row.id_evento),
            fecha_acceso,
        )
        detail_row = cursor.fetchone()
        if detail_row is None:
            connection.rollback()
            raise DatabaseUnavailable("SQL Server no creó el detalle RFID.")

        cursor.execute(
            """
            INSERT INTO dbo.smartlock_rfid_sync (registro_id, id_detalle_evento)
            VALUES (?, ?);
            """,
            record["registroId"],
            int(detail_row.id_detalle_evento),
        )
        connection.commit()
        return True, None
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló la sincronización de un acceso RFID.") from exc
    finally:
        connection.close()


def validate_rfid_online(uid):
    if not RFID_DEVICE_API_KEY:
        return 503, {"error": "La clave del dispositivo RFID no está configurada."}

    url = f"{RFID_API_BASE_URL}/api/acceso/validar-rfid"
    body = json.dumps({"uid": uid}, separators=(",", ":")).encode("utf-8")
    request = Request(
        url,
        data=body,
        headers={
            "Content-Type": "application/json",
            "X-Smartlock-Device-Key": RFID_DEVICE_API_KEY,
        },
        method="POST",
    )
    try:
        with urlopen(request, timeout=8) as response:
            status = response.status
            response_body = response.read(MAX_REQUEST_BYTES + 1)
    except HTTPError as exc:
        status = exc.code
        response_body = exc.read(MAX_REQUEST_BYTES + 1)
    except (TimeoutError, URLError, OSError) as exc:
        logging.warning(
            "No se pudo contactar la API RFID en %s: %s",
            url,
            exc,
        )
        return 503, {"error": "La API RFID no está disponible."}

    if len(response_body) > MAX_REQUEST_BYTES:
        return 502, {"error": "La respuesta de la API RFID es demasiado grande."}
    try:
        result = json.loads(response_body.decode("utf-8"))
    except (UnicodeDecodeError, json.JSONDecodeError):
        return 502, {"error": "La API RFID devolvió una respuesta inválida."}
    if not isinstance(result, dict):
        return 502, {"error": "La API RFID devolvió una respuesta inválida."}
    return status, result


def store_rejection(record):
    fecha_epoch = record["fechaAccesoSqlEpoch"]
    fecha_intento = (
        _parse_access_datetime(fecha_epoch)
        if fecha_epoch > 0
        else None
    )
    id_detalle_evento = record["idDetalleEvento"] or None

    consulta = """
        IF NOT EXISTS (
            SELECT 1
            FROM dbo.smartlock_qr_rechazos WITH (UPDLOCK, HOLDLOCK)
            WHERE registro_id = CONVERT(uniqueidentifier, ?)
        )
        BEGIN
            INSERT INTO dbo.smartlock_qr_rechazos
                (registro_id, id_detalle_evento, codigo_qr_hash, motivo, fecha_intento)
            VALUES
                (CONVERT(uniqueidentifier, ?), ?, ?, ?, COALESCE(?, GETDATE()));
        END;
    """

    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute(
            consulta,
            record["registroId"],
            record["registroId"],
            id_detalle_evento,
            record["codigoQrHash"],
            record["motivo"],
            fecha_intento,
        )
        connection.commit()
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló el registro del QR rechazado en SQL Server.") from exc
    finally:
        connection.close()


def store_rfid_rejection(record):
    fecha_epoch = record["fechaAccesoSqlEpoch"]
    fecha_intento = (
        _parse_access_datetime(fecha_epoch)
        if fecha_epoch > 0
        else None
    )
    id_usuario = record["idUsuario"] or None

    consulta = """
        SET XACT_ABORT ON;
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.smartlock_accesos_rechazados WITH (UPDLOCK, HOLDLOCK)
            WHERE registro_id = CONVERT(uniqueidentifier, ?)
        )
        BEGIN
            INSERT INTO dbo.smartlock_accesos_rechazados
                (registro_id, id_usuario, metodo_acceso, credencial_hash,
                 motivo, fecha_acceso)
            VALUES
                (CONVERT(uniqueidentifier, ?), ?, 'RFID', ?, ?, COALESCE(?, GETDATE()));
        END;

        COMMIT TRANSACTION;
    """

    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute(
            consulta,
            record["registroId"],
            record["registroId"],
            id_usuario,
            record["codigoQrHash"],
            record["motivo"],
            fecha_intento,
        )
        connection.commit()
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló el registro del acceso RFID rechazado en SQL Server.") from exc
    finally:
        connection.close()


def store_reconnection(record):
    fecha_epoch = record["fechaAccesoSqlEpoch"]
    fecha_incidente = (
        _parse_access_datetime(fecha_epoch)
        if fecha_epoch > 0
        else None
    )

    consulta = """
        SET XACT_ABORT ON;
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.smartlock_reconexion_sync WITH (UPDLOCK, HOLDLOCK)
            WHERE registro_id = CONVERT(uniqueidentifier, ?)
        )
        BEGIN
            DECLARE @FechaIncidente datetime = COALESCE(?, GETDATE());
            INSERT INTO dbo.incidentes
                (tipo_incidente, descripcion_incidente, prioridad_incidente,
                 estado_incidente, fecha_incidente, hora_incidente,
                 id_usuario_registro)
            VALUES
                ('Conexión restablecida',
                 'La puerta restableció su conexión a internet.',
                 'Alta', 'Resuelto', CONVERT(date, @FechaIncidente),
                 CONVERT(time, @FechaIncidente), NULL);

            INSERT INTO dbo.smartlock_reconexion_sync (registro_id, id_incidente)
            VALUES (CONVERT(uniqueidentifier, ?), CONVERT(int, SCOPE_IDENTITY()));
        END;

        COMMIT TRANSACTION;
    """

    connection = open_database()
    try:
        cursor = connection.cursor()
        cursor.execute(
            consulta,
            record["registroId"],
            fecha_incidente,
            record["registroId"],
        )
        connection.commit()
    except ODBC_ERROR as exc:
        connection.rollback()
        raise DatabaseUnavailable("Falló el registro del incidente de reconexión en SQL Server.") from exc
    finally:
        connection.close()


def validate_sync_record(record):
    if not isinstance(record, dict):
        raise ValueError("Cada registro debe ser un objeto JSON.")

    record_id = record.get("registroId")
    if not isinstance(record_id, str):
        raise ValueError("registroId debe ser un UUID.")
    try:
        canonical_id = str(uuid.UUID(record_id))
    except (TypeError, ValueError, AttributeError) as exc:
        raise ValueError("registroId debe ser un UUID.") from exc
    if canonical_id != record_id.lower():
        raise ValueError("registroId debe estar en formato UUID.")

    record_type = record.get("tipo")
    if record_type == "rfid":
        user_id = record.get("idUsuario")
        if not isinstance(user_id, int) or isinstance(user_id, bool) or user_id <= 0:
            raise ValueError("idUsuario inválido.")

        access_epoch = record.get("fechaAccesoSqlEpoch")
        snapshot_epoch = record.get("snapshotSqlEpoch")
        if (
            not isinstance(access_epoch, int)
            or isinstance(access_epoch, bool)
            or not isinstance(snapshot_epoch, int)
            or isinstance(snapshot_epoch, bool)
            or access_epoch < snapshot_epoch
            or access_epoch - snapshot_epoch >= CACHE_TTL_SECONDS
        ):
            raise ValueError("El acceso RFID está fuera de la vigencia de caché.")
        _parse_access_datetime(access_epoch)
        _parse_access_datetime(snapshot_epoch)
        return {
            "registroId": canonical_id,
            "tipo": "rfid",
            "idUsuario": user_id,
            "snapshotSqlEpoch": snapshot_epoch,
            "fechaAccesoSqlEpoch": access_epoch,
        }

    if record_type == "acceso":
        detail_id = record.get("idDetalleEvento")
        qr_code = record.get("codigoQr")
        if not isinstance(detail_id, int) or isinstance(detail_id, bool) or detail_id <= 0:
            raise ValueError("idDetalleEvento inválido.")
        if not isinstance(qr_code, str) or len(qr_code) != 36:
            raise ValueError("codigoQr inválido.")
        try:
            if str(uuid.UUID(qr_code)) != qr_code.lower():
                raise ValueError("codigoQr debe estar en formato UUID.")
        except (ValueError, AttributeError) as exc:
            raise ValueError("codigoQr debe ser un UUID.") from exc

        user_id = record.get("idUsuario")
        event_id = record.get("idEvento")
        if not isinstance(user_id, int) or isinstance(user_id, bool) or user_id <= 0:
            raise ValueError("idUsuario inválido.")
        if not isinstance(event_id, int) or isinstance(event_id, bool) or event_id <= 0:
            raise ValueError("idEvento inválido.")
        if record.get("estadoUsuario") is not True:
            raise ValueError("El usuario no estaba activo según la caché.")
        if record.get("estadoEvento") is not True:
            raise ValueError("El evento no estaba activo según la caché.")

        access_epoch = record.get("fechaAccesoSqlEpoch")
        snapshot_epoch = record.get("snapshotSqlEpoch")
        if (
            not isinstance(access_epoch, int)
            or isinstance(access_epoch, bool)
            or not isinstance(snapshot_epoch, int)
            or isinstance(snapshot_epoch, bool)
            or access_epoch < snapshot_epoch
            or access_epoch - snapshot_epoch >= CACHE_TTL_SECONDS
        ):
            raise ValueError("La fecha del acceso está fuera de la vigencia de caché.")
        _parse_access_datetime(access_epoch)
        return {
            "registroId": canonical_id,
            "tipo": "acceso",
            "idDetalleEvento": detail_id,
            "codigoQr": qr_code.lower(),
            "idUsuario": user_id,
            "idEvento": event_id,
            "estadoUsuario": True,
            "estadoEvento": True,
            "snapshotSqlEpoch": snapshot_epoch,
            "fechaAccesoSqlEpoch": access_epoch,
        }

    if record_type == "rechazo":
        qr_hash = record.get("codigoQrHash", "")
        if not isinstance(qr_hash, str) or len(qr_hash) > 128:
            raise ValueError("codigoQrHash inválido.")
        access_epoch = record.get("fechaAccesoSqlEpoch", 0)
        if (
            not isinstance(access_epoch, int)
            or isinstance(access_epoch, bool)
            or access_epoch < 0
        ):
            raise ValueError("fechaAccesoSqlEpoch inválida.")
        if access_epoch > 0:
            _parse_access_datetime(access_epoch)
        detail_id = record.get("idDetalleEvento")
        if detail_id is not None and (
            not isinstance(detail_id, int)
            or isinstance(detail_id, bool)
            or detail_id < 0
        ):
            raise ValueError("idDetalleEvento inválido.")
        motivo = record.get("motivo", "rechazado")
        if not isinstance(motivo, str) or len(motivo) > 80:
            raise ValueError("motivo inválido.")
        return {
            "registroId": canonical_id,
            "tipo": "rechazo",
            "fechaAccesoSqlEpoch": access_epoch,
            "idDetalleEvento": detail_id,
            "codigoQrHash": qr_hash,
            "motivo": motivo,
        }

    if record_type == "rechazo-rfid":
        uid_hash = record.get("codigoQrHash", "")
        if (
            not isinstance(uid_hash, str)
            or len(uid_hash) != 16
            or any(caracter not in "0123456789abcdefABCDEF" for caracter in uid_hash)
        ):
            raise ValueError("codigoQrHash RFID inválido.")
        access_epoch = record.get("fechaAccesoSqlEpoch", 0)
        if (
            not isinstance(access_epoch, int)
            or isinstance(access_epoch, bool)
            or access_epoch < 0
        ):
            raise ValueError("fechaAccesoSqlEpoch inválida.")
        if access_epoch > 0:
            _parse_access_datetime(access_epoch)
        motivo = record.get("motivo", "tarjeta_no_autorizada_offline")
        if not isinstance(motivo, str) or len(motivo) > 80:
            raise ValueError("motivo inválido.")
        return {
            "registroId": canonical_id,
            "tipo": "rechazo-rfid",
            "fechaAccesoSqlEpoch": access_epoch,
            "idUsuario": 0,
            "codigoQrHash": uid_hash.lower(),
            "motivo": motivo,
        }

    if record_type == "reconexion":
        access_epoch = record.get("fechaAccesoSqlEpoch", 0)
        if (
            not isinstance(access_epoch, int)
            or isinstance(access_epoch, bool)
            or access_epoch < 0
        ):
            raise ValueError("fechaAccesoSqlEpoch inválida.")
        if access_epoch > 0:
            _parse_access_datetime(access_epoch)
        return {
            "registroId": canonical_id,
            "tipo": "reconexion",
            "fechaAccesoSqlEpoch": access_epoch,
        }

    raise ValueError("tipo de registro no admitido.")


class SmartlockHandler(BaseHTTPRequestHandler):
    server_version = "SMARTLOCK-Local/1.0"

    def _send_json(self, status_code, payload):
        body = json.dumps(payload, ensure_ascii=False, separators=(",", ":")).encode(
            "utf-8"
        )
        self.send_response(status_code)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Cache-Control", "no-store")
        self.end_headers()
        self.wfile.write(body)

    def _authorized(self):
        if not HTTP_USERNAME or not HTTP_PASSWORD:
            self._send_json(503, {"error": "Autenticación HTTP no configurada."})
            return False

        header = self.headers.get("Authorization", "")
        scheme, separator, encoded = header.partition(" ")
        if not separator or scheme.lower() != "basic":
            self._send_json(401, {"error": "Autenticación requerida."})
            return False

        try:
            received = base64.b64decode(encoded, validate=True)
        except (ValueError, TypeError):
            self._send_json(401, {"error": "Autenticación inválida."})
            return False

        expected = f"{HTTP_USERNAME}:{HTTP_PASSWORD}".encode("utf-8")
        if not hmac.compare_digest(received, expected):
            self._send_json(401, {"error": "Autenticación inválida."})
            return False
        return True

    def do_GET(self):
        if not self._authorized():
            return

        path = urlsplit(self.path).path
        if path == "/estado":
            self._get_status()
        elif path == "/qr-autorizados":
            self._get_authorized_qrs()
        else:
            self._send_json(404, {"error": "Ruta no encontrada."})

    def _get_status(self):
        payload = {
            "servidorDisponible": True,
            "sqlDisponible": False,
            "serverUtcEpoch": int(time.time()),
            "sqlNowEpoch": None,
        }
        try:
            connection = open_database()
            try:
                cursor = connection.cursor()
                cursor.execute("SELECT GETDATE() AS fecha_actual;")
                row = cursor.fetchone()
                if row is None or row.fecha_actual is None:
                    raise DatabaseUnavailable("SQL Server no devolvió la hora actual.")
                payload["sqlDisponible"] = True
                payload["sqlNowEpoch"] = _sql_wall_epoch(row.fecha_actual)
                connection.commit()
            finally:
                connection.close()
        except DatabaseUnavailable as exc:
            logging.warning("Estado SQL no disponible: %s", exc)
            payload["detalleSql"] = str(exc)
        except ODBC_ERROR as exc:
            logging.warning("Estado SQL no disponible: %s", type(exc).__name__)
            payload["detalleSql"] = "No se pudo consultar SQL Server."

        self._send_json(200, payload)

    def _get_authorized_qrs(self):
        try:
            payload = get_authorized_qrs()
        except DatabaseUnavailable as exc:
            logging.warning("No se pudo descargar la caché SQL: %s", exc)
            self._send_json(503, {"error": "SQL Server no disponible."})
            return
        self._send_json(200, payload)

    def do_POST(self):
        if not self._authorized():
            return
        path = urlsplit(self.path).path
        if path not in ("/sincronizar", "/rfid/validar"):
            self._send_json(404, {"error": "Ruta no encontrada."})
            return

        try:
            content_length = int(self.headers.get("Content-Length", "0"))
        except ValueError:
            self._send_json(400, {"error": "Content-Length inválido."})
            return
        if content_length <= 0 or content_length > MAX_REQUEST_BYTES:
            self._send_json(413, {"error": "Tamaño de solicitud inválido."})
            return

        try:
            body = self.rfile.read(content_length)
            payload = json.loads(body.decode("utf-8"))
            if not isinstance(payload, dict):
                raise ValueError("El cuerpo JSON debe ser un objeto.")
            if path == "/rfid/validar":
                uid = payload.get("uid")
                if not isinstance(uid, str) or not uid.strip() or len(uid) > 50:
                    raise ValueError("uid inválido.")
                return self._post_validate_rfid(uid.strip())

            records = payload.get("registros")
            if not isinstance(records, list) or len(records) > MAX_SYNC_RECORDS:
                raise ValueError("registros debe ser una lista de hasta 128 elementos.")
            validated = [validate_sync_record(item) for item in records]
        except (UnicodeDecodeError, json.JSONDecodeError, ValueError) as exc:
            self._send_json(400, {"error": str(exc)})
            return

        confirmed = []
        conflicts = []
        try:
            for record in validated:
                if record["tipo"] == "rechazo":
                    store_rejection(record)
                    confirmed.append(record["registroId"])
                    continue

                if record["tipo"] == "rechazo-rfid":
                    store_rfid_rejection(record)
                    confirmed.append(record["registroId"])
                    continue

                if record["tipo"] == "reconexion":
                    store_reconnection(record)
                    confirmed.append(record["registroId"])
                    continue

                if record["tipo"] == "rfid":
                    stored, reason = sync_rfid_access(record)
                else:
                    stored, reason = sync_access(record)
                if stored:
                    confirmed.append(record["registroId"])
                else:
                    conflicts.append(
                        {"registroId": record["registroId"], "motivo": reason}
                    )
        except DatabaseUnavailable as exc:
            logging.warning("Sincronización SQL interrumpida: %s", exc)
            self._send_json(503, {"error": "SQL Server no disponible."})
            return
        except OSError as exc:
            logging.error("No se pudo persistir el registro local: %s", type(exc).__name__)
            self._send_json(507, {"error": "No se pudo guardar el registro local."})
            return

        self._send_json(
            200,
            {"confirmados": confirmed, "conflictos": conflicts},
        )

    def _post_validate_rfid(self, uid):
        status, payload = validate_rfid_online(uid)
        self._send_json(status, payload)

    def log_message(self, format_string, *args):
        logging.info("%s - %s", self.address_string(), format_string % args)


def main():
    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s %(levelname)s %(message)s",
    )
    if not HTTP_USERNAME or not HTTP_PASSWORD:
        raise SystemExit(
            "Configura SMARTLOCK_HTTP_USERNAME y SMARTLOCK_HTTP_PASSWORD antes de iniciar."
        )

    server = ThreadingHTTPServer((HTTP_HOST, HTTP_PORT), SmartlockHandler)
    server.daemon_threads = True
    logging.info("Servidor SMARTLOCK escuchando en %s:%s", HTTP_HOST, HTTP_PORT)
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        logging.info("Cerrando servidor SMARTLOCK.")
    finally:
        server.server_close()


if __name__ == "__main__":
    main()
