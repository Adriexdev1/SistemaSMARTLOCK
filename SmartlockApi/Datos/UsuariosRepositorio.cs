using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class UsuariosRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<IReadOnlyList<UsuarioResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT u.id_usuario, u.nombre_usuario, u.correo_usuario, u.telefono_usuario,
                   u.id_rol, r.nombre_rol, u.estado_usuario
            FROM dbo.usuarios u
            LEFT JOIN dbo.roles r ON r.id_rol = u.id_rol
            ORDER BY u.nombre_usuario;
            """;

        var usuarios = new List<UsuarioResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            usuarios.Add(MapearUsuario(lector));

        return usuarios;
    }

    public async Task<UsuarioResponse?> ObtenerAsync(int idUsuario, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT u.id_usuario, u.nombre_usuario, u.correo_usuario, u.telefono_usuario,
                   u.id_rol, r.nombre_rol, u.estado_usuario
            FROM dbo.usuarios u
            LEFT JOIN dbo.roles r ON r.id_rol = u.id_rol
            WHERE u.id_usuario = @IdUsuario;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        return await lector.ReadAsync(cancellationToken) ? MapearUsuario(lector) : null;
    }

    public async Task<IReadOnlyList<RolUsuarioResponse>> ListarRolesAsync(CancellationToken cancellationToken)
    {
        const string consulta = "SELECT id_rol, nombre_rol FROM dbo.roles ORDER BY nombre_rol;";
        var roles = new List<RolUsuarioResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            roles.Add(new RolUsuarioResponse(lector.GetInt32(0), lector.GetString(1)));

        return roles;
    }

    public async Task<int> CrearAsync(
        CrearUsuarioRequest request,
        string contrasenaHash,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            INSERT INTO dbo.usuarios
                (nombre_usuario, correo_usuario, contrasena_usuario, id_rol, estado_usuario)
            OUTPUT INSERTED.id_usuario
            VALUES (@Nombre, @Correo, @Contrasena, @IdRol, 1);
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 200).Value = request.NombreUsuario.Trim();
        comando.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = request.CorreoUsuario.Trim();
        comando.Parameters.Add("@Contrasena", SqlDbType.VarChar, 100).Value = contrasenaHash;
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = (object?)request.IdRol ?? DBNull.Value;

        await conexion.OpenAsync(cancellationToken);
        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    public async Task<bool> ActualizarAsync(
        int idUsuario,
        ActualizarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE dbo.usuarios
            SET nombre_usuario = @Nombre,
                correo_usuario = @Correo,
                id_rol = @IdRol,
                estado_usuario = @Estado
            WHERE id_usuario = @IdUsuario;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 200).Value = request.NombreUsuario.Trim();
        comando.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = request.CorreoUsuario.Trim();
        comando.Parameters.Add("@IdRol", SqlDbType.Int).Value = (object?)request.IdRol ?? DBNull.Value;
        comando.Parameters.Add("@Estado", SqlDbType.Int).Value = request.EstadoUsuario;
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> EliminarAsync(int idUsuario, CancellationToken cancellationToken)
    {
        const string consulta = "DELETE FROM dbo.usuarios WHERE id_usuario = @IdUsuario;";
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static UsuarioResponse MapearUsuario(SqlDataReader lector)
    {
        int ordinalIdRol = lector.GetOrdinal("id_rol");
        int ordinalNombreRol = lector.GetOrdinal("nombre_rol");

        return new UsuarioResponse(
            lector.GetInt32(lector.GetOrdinal("id_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_usuario")),
            lector.GetString(lector.GetOrdinal("correo_usuario")),
            lector.IsDBNull(lector.GetOrdinal("telefono_usuario")) ? null : lector.GetString(lector.GetOrdinal("telefono_usuario")),
            lector.IsDBNull(ordinalIdRol) ? null : lector.GetInt32(ordinalIdRol),
            lector.IsDBNull(ordinalNombreRol) ? null : lector.GetString(ordinalNombreRol),
            lector.GetInt32(lector.GetOrdinal("estado_usuario")));
    }
}