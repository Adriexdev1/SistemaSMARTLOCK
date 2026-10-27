using Microsoft.Data.SqlClient;
using System.Data;

namespace SmartlockApi.Datos;

internal sealed record UsuarioCredencial(
    int IdUsuario,
    string NombreUsuario,
    string CorreoUsuario,
    string? TelefonoUsuario,
    string? Contrasena,
    int EstadoUsuario,
    string? Rol,
    string? PinHash);

public sealed class AuthRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    internal async Task<UsuarioCredencial?> BuscarPorCorreoAsync(string correo, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT TOP (1)
                u.id_usuario,
                u.nombre_usuario,
                u.correo_usuario,
                u.telefono_usuario,
                u.contrasena_usuario,
                u.estado_usuario,
                u.pin_usuario,
                r.nombre_rol
            FROM usuarios u
            LEFT JOIN roles r ON r.id_rol = u.id_rol
            WHERE u.correo_usuario = @Correo;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Correo", System.Data.SqlDbType.VarChar, 100).Value = correo;

        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        if (!await lector.ReadAsync(cancellationToken))
            return null;

        return new UsuarioCredencial(
            lector.GetInt32(lector.GetOrdinal("id_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_usuario")),
            lector.GetString(lector.GetOrdinal("correo_usuario")),
            lector.IsDBNull(lector.GetOrdinal("telefono_usuario"))
                ? null
                : lector.GetString(lector.GetOrdinal("telefono_usuario")),
            lector.IsDBNull(lector.GetOrdinal("contrasena_usuario"))
                ? null
                : lector.GetString(lector.GetOrdinal("contrasena_usuario")),
            lector.GetInt32(lector.GetOrdinal("estado_usuario")),
            lector.IsDBNull(lector.GetOrdinal("nombre_rol"))
                ? null
                : lector.GetString(lector.GetOrdinal("nombre_rol")),
            lector.IsDBNull(lector.GetOrdinal("pin_usuario"))
                ? null
                : lector.GetString(lector.GetOrdinal("pin_usuario")));
    }

    internal async Task<UsuarioCredencial?> BuscarPorIdAsync(int idUsuario, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT TOP (1)
                u.id_usuario,
                u.nombre_usuario,
                u.correo_usuario,
                u.telefono_usuario,
                u.contrasena_usuario,
                u.estado_usuario,
                u.pin_usuario,
                r.nombre_rol
            FROM usuarios u
            LEFT JOIN roles r ON r.id_rol = u.id_rol
            WHERE u.id_usuario = @IdUsuario;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdUsuario", System.Data.SqlDbType.Int).Value = idUsuario;

        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        if (!await lector.ReadAsync(cancellationToken))
            return null;

        return new UsuarioCredencial(
            lector.GetInt32(lector.GetOrdinal("id_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_usuario")),
            lector.GetString(lector.GetOrdinal("correo_usuario")),
            lector.IsDBNull(lector.GetOrdinal("telefono_usuario")) ? null : lector.GetString(lector.GetOrdinal("telefono_usuario")),
            lector.IsDBNull(lector.GetOrdinal("contrasena_usuario")) ? null : lector.GetString(lector.GetOrdinal("contrasena_usuario")),
            lector.GetInt32(lector.GetOrdinal("estado_usuario")),
            lector.IsDBNull(lector.GetOrdinal("nombre_rol")) ? null : lector.GetString(lector.GetOrdinal("nombre_rol")),
            lector.IsDBNull(lector.GetOrdinal("pin_usuario")) ? null : lector.GetString(lector.GetOrdinal("pin_usuario")));
    }

    internal async Task ActualizarContrasenaAsync(
        int idUsuario,
        string contrasenaAnterior,
        string nuevoHash,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE usuarios
            SET contrasena_usuario = @NuevoHash
            WHERE id_usuario = @IdUsuario AND contrasena_usuario = @ContrasenaAnterior;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@NuevoHash", System.Data.SqlDbType.VarChar, 100).Value = nuevoHash;
        comando.Parameters.Add("@IdUsuario", System.Data.SqlDbType.Int).Value = idUsuario;
        comando.Parameters.Add("@ContrasenaAnterior", System.Data.SqlDbType.VarChar, 100).Value = contrasenaAnterior;

        await conexion.OpenAsync(cancellationToken);
        await comando.ExecuteNonQueryAsync(cancellationToken);
    }

    internal async Task<bool> GuardarNuevaContrasenaAsync(
        int idUsuario,
        string nuevoHash,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE usuarios
            SET contrasena_usuario = @NuevaContrasena
            WHERE id_usuario = @IdUsuario AND estado_usuario = 1;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@NuevaContrasena", System.Data.SqlDbType.VarChar, 100).Value = nuevoHash;
        comando.Parameters.Add("@IdUsuario", System.Data.SqlDbType.Int).Value = idUsuario;

        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    internal async Task<bool> ActualizarPerfilAsync(
        int idUsuario,
        string nombreUsuario,
        string correoUsuario,
        string? telefonoUsuario,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE usuarios
            SET nombre_usuario = @NombreUsuario,
                correo_usuario = @CorreoUsuario,
                telefono_usuario = @TelefonoUsuario
            WHERE id_usuario = @IdUsuario AND estado_usuario = 1;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@NombreUsuario", SqlDbType.VarChar, 200).Value = nombreUsuario;
        comando.Parameters.Add("@CorreoUsuario", SqlDbType.VarChar, 100).Value = correoUsuario;
        comando.Parameters.Add("@TelefonoUsuario", SqlDbType.VarChar, 25).Value =
            string.IsNullOrWhiteSpace(telefonoUsuario) ? DBNull.Value : telefonoUsuario;
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    internal async Task<bool> GuardarPinHashAsync(int idUsuario, string pinHash, CancellationToken cancellationToken)
    {
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand("SP_GUARDAR_PIN_USUARIO", conexion)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        comando.Parameters.Add("@id_usuario", System.Data.SqlDbType.Int).Value = idUsuario;
        comando.Parameters.Add("@pin_hash", System.Data.SqlDbType.VarChar, 100).Value = pinHash;
        var resultado = comando.Parameters.Add("@resultado", System.Data.SqlDbType.Bit);
        resultado.Direction = System.Data.ParameterDirection.Output;

        await conexion.OpenAsync(cancellationToken);
        await comando.ExecuteNonQueryAsync(cancellationToken);
        return resultado.Value is bool actualizado && actualizado;
    }

    internal async Task<bool> EliminarPinAsync(int idUsuario, CancellationToken cancellationToken)
    {
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand("SP_ELIMINAR_PIN_USUARIO", conexion)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        comando.Parameters.Add("@id_usuario", System.Data.SqlDbType.Int).Value = idUsuario;
        var resultado = comando.Parameters.Add("@resultado", System.Data.SqlDbType.Bit);
        resultado.Direction = System.Data.ParameterDirection.Output;

        await conexion.OpenAsync(cancellationToken);
        await comando.ExecuteNonQueryAsync(cancellationToken);
        return resultado.Value is bool actualizado && actualizado;
    }
}