using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class RolesRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<IReadOnlyList<RolResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT id_rol, nombre_rol, descripcion_rol, permiso_rol, estado_rol, fecha_rol
            FROM dbo.roles
            ORDER BY nombre_rol;
            """;

        var roles = new List<RolResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            roles.Add(Mapear(lector));

        return roles;
    }

    public async Task<RolResponse?> ObtenerAsync(int idRol, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT id_rol, nombre_rol, descripcion_rol, permiso_rol, estado_rol, fecha_rol
            FROM dbo.roles
            WHERE id_rol = @IdRol;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        return await lector.ReadAsync(cancellationToken) ? Mapear(lector) : null;
    }

    public async Task<int> CrearAsync(CrearRolRequest request, CancellationToken cancellationToken)
    {
        const string consulta = """
            INSERT INTO dbo.roles (nombre_rol, descripcion_rol, permiso_rol, estado_rol)
            OUTPUT INSERTED.id_rol
            VALUES (@Nombre, @Descripcion, @Permiso, @Estado);
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        AgregarCampos(comando, request.NombreRol, request.DescripcionRol, request.PermisoRol, request.EstadoRol);
        await conexion.OpenAsync(cancellationToken);
        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    public async Task<bool> ActualizarAsync(
        int idRol,
        ActualizarRolRequest request,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE dbo.roles
            SET nombre_rol = @Nombre,
                descripcion_rol = @Descripcion,
                permiso_rol = @Permiso,
                estado_rol = @Estado
            WHERE id_rol = @IdRol;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        AgregarCampos(comando, request.NombreRol, request.DescripcionRol, request.PermisoRol, request.EstadoRol);
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> EliminarAsync(int idRol, CancellationToken cancellationToken)
    {
        const string consulta = "DELETE FROM dbo.roles WHERE id_rol = @IdRol;";
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AgregarCampos(SqlCommand comando, string nombre, string? descripcion, int permiso, int estado)
    {
        comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = nombre.Trim();
        comando.Parameters.Add("@Descripcion", SqlDbType.VarChar, 150).Value =
            string.IsNullOrWhiteSpace(descripcion) ? DBNull.Value : descripcion.Trim();
        comando.Parameters.Add("@Permiso", SqlDbType.Int).Value = permiso;
        comando.Parameters.Add("@Estado", SqlDbType.Int).Value = estado;
    }

    private static RolResponse Mapear(SqlDataReader lector)
    {
        int ordinalFecha = lector.GetOrdinal("fecha_rol");
        DateTimeOffset? fecha = lector.IsDBNull(ordinalFecha)
            ? null
            : new DateTimeOffset(DateTime.SpecifyKind(lector.GetDateTime(ordinalFecha), DateTimeKind.Local));

        return new RolResponse(
            lector.GetInt32(lector.GetOrdinal("id_rol")),
            lector.GetString(lector.GetOrdinal("nombre_rol")),
            lector.IsDBNull(lector.GetOrdinal("descripcion_rol")) ? null : lector.GetString(lector.GetOrdinal("descripcion_rol")),
            lector.IsDBNull(lector.GetOrdinal("permiso_rol")) ? 0 : lector.GetInt32(lector.GetOrdinal("permiso_rol")),
            lector.IsDBNull(lector.GetOrdinal("estado_rol")) ? 0 : lector.GetInt32(lector.GetOrdinal("estado_rol")),
            fecha);
    }
}