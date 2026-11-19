using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class AuthService(AuthRepositorio repositorio, RfidCardHasher tarjetaHasher) : IAuthService
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
            RfidCardHasher.EsHashDeTarjeta(usuario.TarjetaHash),
            usuario.PermisoRol);
    }

    public async Task<ResultadoConfigurarTarjetaRfid> ConfigurarTarjetaRfidAsync(
        ConfigurarTarjetaRfidRequest request,
        CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null)
            return ResultadoConfigurarTarjetaRfid.UsuarioNoEncontrado;
        if (usuario.PermisoRol != 2)
            return ResultadoConfigurarTarjetaRfid.SinPermiso;

        if (!tarjetaHasher.EstaConfigurado)
            return ResultadoConfigurarTarjetaRfid.ServicioNoConfigurado;

        if (!tarjetaHasher.TryHashUid(request.Uid, out string hashTarjeta))
            return ResultadoConfigurarTarjetaRfid.UidInvalido;

        if (!await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return ResultadoConfigurarTarjetaRfid.CredencialesInvalidas;

        bool guardada = await repositorio.GuardarTarjetaHashAsync(
            usuario.IdUsuario,
            hashTarjeta,
            cancellationToken);
        return guardada
            ? ResultadoConfigurarTarjetaRfid.Configurada
            : ResultadoConfigurarTarjetaRfid.UsuarioNoEncontrado;
    }

    public async Task<ResultadoEliminarTarjetaRfid> EliminarTarjetaRfidAsync(
        EliminarTarjetaRfidRequest request,
        CancellationToken cancellationToken)
    {
        UsuarioCredencial? usuario = await repositorio.BuscarPorIdAsync(request.IdUsuario, cancellationToken);
        if (usuario is null)
            return ResultadoEliminarTarjetaRfid.UsuarioNoEncontrado;
        if (usuario.PermisoRol != 2)
            return ResultadoEliminarTarjetaRfid.SinPermiso;
        if (!await ValidarContrasenaAsync(usuario, request.ContrasenaActual, cancellationToken))
            return ResultadoEliminarTarjetaRfid.CredencialesInvalidas;

        return await repositorio.EliminarTarjetaRfidAsync(usuario.IdUsuario, cancellationToken)
            ? ResultadoEliminarTarjetaRfid.Eliminada
            : ResultadoEliminarTarjetaRfid.UsuarioNoEncontrado;
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