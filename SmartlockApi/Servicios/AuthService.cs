using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class AuthService(AuthRepositorio repositorio) : IAuthService
{
    public async Task<LoginResponse?> AutenticarAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorCorreoAsync(request.Correo, cancellationToken);
        if (usuario is null || !await ValidarContrasenaAsync(usuario, request.Contrasena, cancellationToken))
            return null;

        return new LoginResponse(
            usuario.IdUsuario,
            usuario.NombreUsuario,
            usuario.CorreoUsuario,
            usuario.TelefonoUsuario,
            usuario.Rol,
            !string.IsNullOrEmpty(usuario.PinHash));
    }

    public async Task<bool> ConfigurarPinAsync(ConfigurarPinRequest request, CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null || !await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return false;

        return await repositorio.GuardarPinHashAsync(
            usuario.IdUsuario,
            PasswordHasher.Hash(request.Pin),
            cancellationToken);
    }

    public async Task<bool> EliminarPinAsync(EliminarPinRequest request, CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null || !await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return false;

        return await repositorio.EliminarPinAsync(usuario.IdUsuario, cancellationToken);
    }

    public async Task<bool> CambiarContrasenaAsync(
        CambiarContrasenaRequest request,
        CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null || !await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return false;

        return await repositorio.GuardarNuevaContrasenaAsync(
            usuario.IdUsuario,
            PasswordHasher.Hash(request.NuevaContrasena),
            cancellationToken);
    }

    public async Task<ResultadoActualizarPerfil> ActualizarPerfilAsync(
        ActualizarPerfilRequest request,
        CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null)
            return ResultadoActualizarPerfil.UsuarioNoEncontrado;

        if (!await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return ResultadoActualizarPerfil.CredencialesInvalidas;

        try
        {
            bool actualizado = await repositorio.ActualizarPerfilAsync(
                usuario.IdUsuario,
                request.NombreUsuario.Trim(),
                request.CorreoUsuario.Trim(),
                request.TelefonoUsuario?.Trim(),
                cancellationToken);

            return actualizado
                ? ResultadoActualizarPerfil.Actualizado
                : ResultadoActualizarPerfil.UsuarioNoEncontrado;
        }
        catch (SqlException excepcion) when (excepcion.Number is 2601 or 2627)
        {
            return ResultadoActualizarPerfil.CorreoEnUso;
        }
    }

    private async Task<bool> ValidarContrasenaAsync(
        UsuarioCredencial usuario,
        string contrasena,
        CancellationToken cancellationToken)
    {
        if (usuario.EstadoUsuario != 1 || string.IsNullOrEmpty(usuario.Contrasena))
            return false;

        if (PasswordHasher.EsHashAlmacenado(usuario.Contrasena))
            return PasswordHasher.Verificar(contrasena, usuario.Contrasena);

        if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(contrasena),
                Encoding.UTF8.GetBytes(usuario.Contrasena)))
        {
            return false;
        }

        await repositorio.ActualizarContrasenaAsync(
            usuario.IdUsuario,
            usuario.Contrasena,
            PasswordHasher.Hash(contrasena),
            cancellationToken);
        return true;
    }
}