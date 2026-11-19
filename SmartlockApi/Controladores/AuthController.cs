using Microsoft.AspNetCore.Mvc;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Valida las credenciales de un usuario activo.</summary>
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        LoginResponse? usuario = await authService.AutenticarAsync(request, cancellationToken);
        return usuario is null ? Unauthorized() : Ok(usuario);
    }

    /// <summary>Vincula o reemplaza la tarjeta RFID después de verificar la contraseña actual.</summary>
    [HttpPost("tarjeta-rfid")]
    [ProducesResponseType<EstadoTarjetaRfidResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<EstadoTarjetaRfidResponse>> ConfigurarTarjetaRfid(
        [FromBody] ConfigurarTarjetaRfidRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            ResultadoConfigurarTarjetaRfid resultado =
                await authService.ConfigurarTarjetaRfidAsync(request, cancellationToken);
            return resultado switch
            {
                ResultadoConfigurarTarjetaRfid.Configurada => Ok(new EstadoTarjetaRfidResponse(true)),
                ResultadoConfigurarTarjetaRfid.CredencialesInvalidas => Unauthorized(),
                ResultadoConfigurarTarjetaRfid.UidInvalido => BadRequest(new { mensaje = "El UID debe contener 4, 7 o 10 bytes en hexadecimal." }),
                ResultadoConfigurarTarjetaRfid.UsuarioNoEncontrado => NotFound(),
                ResultadoConfigurarTarjetaRfid.SinPermiso => Forbid(),
                ResultadoConfigurarTarjetaRfid.ServicioNoConfigurado => StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new { mensaje = "El servicio de tarjetas RFID no está configurado." }),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
        catch (Microsoft.Data.SqlClient.SqlException exception) when (exception.Number is 2601 or 2627)
        {
            return Conflict(new { mensaje = "Esa tarjeta ya está vinculada a otro usuario." });
        }
    }

    /// <summary>Quita la tarjeta RFID después de verificar la contraseña actual.</summary>
    [HttpDelete("tarjeta-rfid")]
    [ProducesResponseType<EstadoTarjetaRfidResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstadoTarjetaRfidResponse>> EliminarTarjetaRfid(
        [FromBody] EliminarTarjetaRfidRequest request,
        CancellationToken cancellationToken)
    {
        ResultadoEliminarTarjetaRfid resultado =
            await authService.EliminarTarjetaRfidAsync(request, cancellationToken);
        return resultado switch
        {
            ResultadoEliminarTarjetaRfid.Eliminada => Ok(new EstadoTarjetaRfidResponse(false)),
            ResultadoEliminarTarjetaRfid.CredencialesInvalidas => Unauthorized(),
            ResultadoEliminarTarjetaRfid.SinPermiso => Forbid(),
            ResultadoEliminarTarjetaRfid.UsuarioNoEncontrado => NotFound(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    /// <summary>Cambia la contraseña después de verificar la contraseña actual.</summary>
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CambiarContrasena(
        [FromBody] CambiarContrasenaRequest request,
        CancellationToken cancellationToken)
    {
        bool actualizado = await authService.CambiarContrasenaAsync(request, cancellationToken);
        return actualizado ? NoContent() : Unauthorized();
    }

    /// <summary>Actualiza el nombre, correo y teléfono del usuario autenticado.</summary>
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarPerfil(
        [FromBody] ActualizarPerfilRequest request,
        CancellationToken cancellationToken)
    {
        ResultadoActualizarPerfil resultado = await authService.ActualizarPerfilAsync(request, cancellationToken);
        return resultado switch
        {
            ResultadoActualizarPerfil.Actualizado => NoContent(),
            ResultadoActualizarPerfil.CredencialesInvalidas => Unauthorized(),
            ResultadoActualizarPerfil.UsuarioNoEncontrado => NotFound(),
            ResultadoActualizarPerfil.CorreoEnUso => Conflict(new { mensaje = "Ese correo ya está registrado." }),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}