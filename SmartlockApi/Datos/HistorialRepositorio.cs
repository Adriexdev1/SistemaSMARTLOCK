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
            SELECT de.id_detalle_evento, de.acceso_detalle_evento,
                   u.nombre_usuario, de.metodo_acceso
            FROM dbo.detalle_eventos de
            INNER JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
            WHERE de.acceso_detalle_evento IS NOT NULL
            ORDER BY de.acceso_detalle_evento DESC;
            """;

        var registros = new List<HistorialResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
        {
            var fecha = lector.GetDateTime(lector.GetOrdinal("acceso_detalle_evento"));
            int idDetalleEvento = lector.GetInt32(lector.GetOrdinal("id_detalle_evento"));
            int ordinalMetodo = lector.GetOrdinal("metodo_acceso");
            registros.Add(new HistorialResponse(
                idDetalleEvento,
                fecha.ToString("yyyy-MM-dd"),
                fecha.ToString("HH:mm:ss"),
                lector.GetString(lector.GetOrdinal("nombre_usuario")),
                idDetalleEvento,
                "Autorizado",
                lector.IsDBNull(ordinalMetodo) ? "No registrado" : lector.GetString(ordinalMetodo)));
        }

        return registros;
    }
}