using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class IncidentesRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<IReadOnlyList<IncidenteResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT i.id_incidente, i.tipo_incidente, i.descripcion_incidente,
                   i.prioridad_incidente, i.estado_incidente, i.fecha_incidente,
                   i.hora_incidente, u.nombre_usuario AS usuario_registro
            FROM dbo.incidentes i
            LEFT JOIN dbo.usuarios u ON u.id_usuario = i.id_usuario_registro
            ORDER BY i.fecha_incidente DESC, i.hora_incidente DESC;
            """;

        var incidentes = new List<IncidenteResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            incidentes.Add(MapearIncidente(lector));

        return incidentes;
    }

    public async Task<IncidenteResponse?> ObtenerAsync(int idIncidente, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT i.id_incidente, i.tipo_incidente, i.descripcion_incidente,
                   i.prioridad_incidente, i.estado_incidente, i.fecha_incidente,
                   i.hora_incidente, u.nombre_usuario AS usuario_registro
            FROM dbo.incidentes i
            LEFT JOIN dbo.usuarios u ON u.id_usuario = i.id_usuario_registro
            WHERE i.id_incidente = @IdIncidente;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdIncidente", SqlDbType.Int).Value = idIncidente;
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        return await lector.ReadAsync(cancellationToken) ? MapearIncidente(lector) : null;
    }

    public async Task<int> CrearAsync(CrearIncidenteRequest request, CancellationToken cancellationToken)
    {
        const string consulta = """
            INSERT INTO dbo.incidentes
                (tipo_incidente, descripcion_incidente, prioridad_incidente,
                 estado_incidente, id_usuario_registro)
            OUTPUT INSERTED.id_incidente
            VALUES (@Tipo, @Descripcion, @Prioridad, 'Pendiente', @IdUsuarioRegistro);
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Tipo", SqlDbType.VarChar, 80).Value = request.Tipo.Trim();
        comando.Parameters.Add("@Descripcion", SqlDbType.VarChar, 500).Value = request.Descripcion.Trim();
        comando.Parameters.Add("@Prioridad", SqlDbType.VarChar, 20).Value = request.Prioridad;
        comando.Parameters.Add("@IdUsuarioRegistro", SqlDbType.Int).Value = request.IdUsuarioRegistro;
        await conexion.OpenAsync(cancellationToken);
        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    public async Task<bool> ActualizarEstadoAsync(
        int idIncidente,
        string estado,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE dbo.incidentes
            SET estado_incidente = @Estado,
                fecha_actualizacion = GETDATE()
            WHERE id_incidente = @IdIncidente;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Estado", SqlDbType.VarChar, 20).Value = estado;
        comando.Parameters.Add("@IdIncidente", SqlDbType.Int).Value = idIncidente;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static IncidenteResponse MapearIncidente(SqlDataReader lector)
    {
        var fecha = lector.GetDateTime(lector.GetOrdinal("fecha_incidente"));
        var hora = lector.GetTimeSpan(lector.GetOrdinal("hora_incidente"));
        int ordinalUsuario = lector.GetOrdinal("usuario_registro");

        return new IncidenteResponse(
            lector.GetInt32(lector.GetOrdinal("id_incidente")),
            lector.GetString(lector.GetOrdinal("tipo_incidente")),
            lector.GetString(lector.GetOrdinal("descripcion_incidente")),
            lector.GetString(lector.GetOrdinal("prioridad_incidente")),
            lector.GetString(lector.GetOrdinal("estado_incidente")),
            fecha.ToString("yyyy-MM-dd"),
            hora.ToString(@"hh\:mm"),
            lector.IsDBNull(ordinalUsuario) ? null : lector.GetString(ordinalUsuario));
    }
}