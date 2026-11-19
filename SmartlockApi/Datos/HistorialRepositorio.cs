using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class HistorialRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<IReadOnlyList<HistorialResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT historial.id, historial.id_detalle_evento, historial.id_evento,
                   historial.fecha, historial.nombre_usuario, historial.resultado,
                   historial.metodo_acceso
            FROM (
                SELECT
                    CAST(de.id_detalle_evento AS BIGINT) AS id,
                    de.id_detalle_evento,
                    e.id_evento,
                    de.acceso_detalle_evento AS fecha,
                    u.nombre_usuario,
                    CAST('Autorizado' AS VARCHAR(20)) AS resultado,
                    COALESCE(de.metodo_acceso, 'No registrado') AS metodo_acceso
                FROM dbo.detalle_eventos de
                INNER JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
                INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
                WHERE de.acceso_detalle_evento IS NOT NULL

                UNION ALL

                SELECT
                    -rechazo.id_rechazo AS id,
                    COALESCE(rechazo.id_detalle_evento, 0) AS id_detalle_evento,
                    COALESCE(e.id_evento, 0) AS id_evento,
                    rechazo.fecha_intento AS fecha,
                    COALESCE(u.nombre_usuario, 'No identificado') AS nombre_usuario,
                    CAST('Rechazado' AS VARCHAR(20)) AS resultado,
                    CAST('Solo QR' AS VARCHAR(20)) AS metodo_acceso
                FROM dbo.smartlock_qr_rechazos rechazo
                LEFT JOIN dbo.detalle_eventos de
                    ON de.id_detalle_evento = rechazo.id_detalle_evento
                LEFT JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
                LEFT JOIN dbo.eventos e ON e.id_evento = de.id_evento

                UNION ALL

                SELECT
                    CAST(-1000000000000 AS BIGINT) - rechazo.id_acceso_rechazado AS id,
                    CAST(0 AS INT) AS id_detalle_evento,
                    CAST(0 AS INT) AS id_evento,
                    rechazo.fecha_acceso AS fecha,
                    COALESCE(u.nombre_usuario, 'No identificado') AS nombre_usuario,
                    CAST('Rechazado' AS VARCHAR(20)) AS resultado,
                    CAST('RFID' AS VARCHAR(20)) AS metodo_acceso
                FROM dbo.smartlock_accesos_rechazados rechazo
                LEFT JOIN dbo.usuarios u ON u.id_usuario = rechazo.id_usuario
                WHERE rechazo.metodo_acceso = 'RFID'
            ) historial
            ORDER BY historial.fecha DESC, historial.id DESC;
            """;

        var registros = new List<HistorialResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
        {
            var fecha = lector.GetDateTime(lector.GetOrdinal("fecha"));
            long id = lector.GetInt64(lector.GetOrdinal("id"));
            int idDetalleEvento = lector.GetInt32(lector.GetOrdinal("id_detalle_evento"));
            int idEvento = lector.GetInt32(lector.GetOrdinal("id_evento"));
            registros.Add(new HistorialResponse(
                id,
                fecha.ToString("yyyy-MM-dd"),
                fecha.ToString("HH:mm:ss"),
                lector.GetString(lector.GetOrdinal("nombre_usuario")),
                idDetalleEvento,
                idEvento,
                lector.GetString(lector.GetOrdinal("resultado")),
                lector.GetString(lector.GetOrdinal("metodo_acceso"))));
        }

        return registros;
    }
}