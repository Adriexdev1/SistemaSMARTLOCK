# Acceso con tarjeta RFID

## Base de datos

La arquitectura reutiliza `usuarios.pin_usuario`: el UID no se guarda en claro, sino como `RFID:` seguido de un HMAC-SHA256. No se crea una tabla de tarjetas ni se sustituye `detalle_eventos`; la tabla auxiliar `smartlock_rfid_sync` sólo evita duplicar accesos offline al reintentar la sincronización. La migración agrega un índice único filtrado sobre `pin_usuario` para impedir que una tarjeta se vincule a más de un usuario; si el prechequeo encuentra valores duplicados existentes, detiene la migración sin borrar datos. Los PIN históricos quedan ignorados y no se validan. Cada lectura autorizada inserta una fila en `detalle_eventos` con `metodo_acceso = 'RFID'`.

En la base existente `SMARTLOCK_BD`, ejecutar una sola vez `SMARTLOCK_RFID_Migracion.sql` después de respaldarla. La migración agrega un índice auxiliar para impedir que una tarjeta quede vinculada a más de un usuario y crea el evento permanente `Acceso permanente RFID`. No ejecutar `SMARTLOCK_181126.sql` para actualizar una base existente: ese archivo crea la base completa.

Para guardar en SQL Server los intentos de QR rechazados por el lector, ejecutar también `SMARTLOCK_QR_Rechazos_Migracion.sql`. Al sincronizar la cola, `SMARTLOCK_Server.py` inserta el intento en `dbo.smartlock_qr_rechazos`; el identificador de registro único evita duplicados si se reintenta. Se conserva el hash del QR, no el código original. El endpoint `/api/historial` y la página Historial muestran estos intentos junto con los accesos autorizados. Si SQL Server no está disponible, el intento permanece pendiente en LittleFS y no se confirma al lector.

Para registrar las tarjetas RFID no autorizadas y los incidentes de reconexión, ejecutar también `SMARTLOCK_AccesosNfc_Migracion.sql` en `SMARTLOCK_BD`. Los rechazos RFID se agregan al historial como `Rechazado / RFID`; la API guarda el HMAC del UID y, si el rechazo ocurre sin conexión, el lector encola una huella de 64 bits para sincronizarla después. Esta huella offline no es un HMAC ni debe tratarse como protección criptográfica. La cola usa identificadores únicos para evitar duplicados al reintentar. El firmware considera restablecida la conexión cuando vuelve a consultar SQL por medio del servidor local; en ese momento encola un incidente `Alta / Resuelto`, que también se deduplica al sincronizar.

## Configuración de la API y el servidor local

Configurar fuera del código dos secretos distintos, cada uno de al menos 32 bytes:

- `Rfid__HmacSecret`: clave privada para derivar el hash almacenado a partir del UID.
- `Rfid__DeviceApiKey`: clave de acceso compartida entre la API y el lector.

Mantener estable `Rfid__HmacSecret`: cambiarlo invalida las tarjetas vinculadas y obliga a registrarlas de nuevo. La API acepta UID hexadecimal de 4, 7 o 10 bytes, con espacios, guiones o dos puntos opcionales. En el portal, cada usuario puede vincular o quitar una tarjeta desde su perfil; para ambas operaciones debe volver a confirmar su contraseña. Un UID vinculado a otro usuario produce `409 Conflict`.

El servidor local `SMARTLOCK_Server.py` necesita `SMARTLOCK_RFID_DEVICE_API_KEY` con el mismo valor configurado en `Rfid__DeviceApiKey`. Por defecto, reenvía a `http://127.0.0.1:5000` (la API local); para producción, establece `SMARTLOCK_RFID_API_BASE_URL` a la URL HTTPS real donde esté desplegada la API. El secreto HMAC nunca se copia al ESP32. No guardes el valor de la clave en el código fuente.

En Windows, puedes cargar la clave que ya está en User Secrets al entorno del usuario sin imprimirla:

```powershell
$secretsPath = Join-Path $env:APPDATA 'Microsoft\UserSecrets\b5b4c30c-4923-4a98-b0de-79604fa90110\secrets.json'
$secrets = Get-Content -Raw $secretsPath | ConvertFrom-Json
$deviceKey = $secrets.'Rfid:DeviceApiKey'
if ([string]::IsNullOrWhiteSpace($deviceKey)) { throw 'No está configurada Rfid:DeviceApiKey.' }
[Environment]::SetEnvironmentVariable('SMARTLOCK_RFID_DEVICE_API_KEY', $deviceKey, 'User')
$env:SMARTLOCK_RFID_DEVICE_API_KEY = $deviceKey
```

Reinicia `SMARTLOCK_Server.py` para que lea la variable.

El UID identifica la tarjeta, pero no es una prueba criptográfica de autenticidad; tarjetas con UID clonable pueden copiarse. El HMAC protege el UID almacenado, no evita la clonación física.

## Contratos

- `POST /api/auth/tarjeta-rfid`: `{ "idUsuario": 1, "contrasenaActual": "...", "uid": "A1 B2 C3 D4" }`
- `DELETE /api/auth/tarjeta-rfid`: `{ "idUsuario": 1, "contrasenaActual": "..." }`
- `POST /api/acceso/validar-rfid`: `{ "uid": "A1 B2 C3 D4" }`, con el encabezado `X-Smartlock-Device-Key`. La respuesta autorizada incluye `idUsuario` para crear la caché local del lector.

La validación RFID sólo requiere que el usuario esté activo; no comprueba su rol. El endpoint de hardware requiere la clave de dispositivo y registra el acceso de manera transaccional. El flujo QR ya no solicita ni valida PIN. El firmware conserva por una hora los UID que autorizó en línea; los accesos offline se encolan y se guardan en `detalle_eventos` al sincronizarse. `smartlock_rfid_sync` es únicamente un registro de idempotencia para evitar insertar dos veces el mismo acceso offline; no reemplaza el historial de `detalle_eventos`.

La caché offline sólo aprende una tarjeta después de una autorización en línea. Una tarjeta revocada podría seguir aceptándose localmente hasta que expire su caché (máximo una hora); una vez sincronizado el servidor local, nuevos intentos se validan en línea. El reloj debe haberse sincronizado antes de perder la conexión; tras un reinicio sin red, el lector falla cerrado si no puede confiar en la hora.

## Lector MFRC522

Instala la biblioteca Arduino MFRC522 (Miguel Balboa). Usa `prototipo_v8_RFID.ino`, basado en `prototipo_v7_synchro.ino` para conservar su sincronización y descarga de caché. Conecta el lector a 3.3 V, no a 5 V: SS/SDA a GPIO 33, SCK a GPIO 14, MOSI a GPIO 23, MISO a GPIO 19 y RST a GPIO 32. Se usa GPIO 14 para SCK porque el servo ya ocupa GPIO 18. El ESP32 conserva hasta 128 tarjetas aprendidas durante una hora y encola sus accesos offline en LittleFS.
