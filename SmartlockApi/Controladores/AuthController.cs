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

    /// <summary>Configura o cambia el PIN después de verificar la contraseña actual.</summary>
    [HttpPost("pin")]
    [ProducesResponseType<EstadoPinResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EstadoPinResponse>> ConfigurarPin(
        [FromBody] ConfigurarPinRequest request,
        CancellationToken cancellationToken)
    {
        bool actualizado = await authService.ConfigurarPinAsync(request, cancellationToken);
        return actualizado
            ? Ok(new EstadoPinResponse(true))
            : Unauthorized();
    }

    /// <summary>Quita el PIN después de verificar la contraseña actual.</summary>
    [HttpDelete("pin")]
    [ProducesResponseType<EstadoPinResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EstadoPinResponse>> EliminarPin(
        [FromBody] EliminarPinRequest request,
        CancellationToken cancellationToken)
    {
        bool actualizado = await authService.EliminarPinAsync(request, cancellationToken);
        return actualizado
            ? Ok(new EstadoPinResponse(false))
            : Unauthorized();
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