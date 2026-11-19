using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/whatsapp/citas")]
public sealed class CitasWhatsAppController(
    CitasWhatsAppRepositorio repositorio,
    IConfiguration configuration,
    ILogger<CitasWhatsAppController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CrearCitaWhatsAppResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CrearCitaWhatsAppResponse>> Crear(
        [FromBody] CrearCitaWhatsAppRequest request,
        CancellationToken cancellationToken)
    {
        string? claveEsperada = configuration["WhatsApp:ApiKey"];
        if (string.IsNullOrWhiteSpace(claveEsperada))
        {
            logger.LogError("No está configurada la clave de acceso de la API para WhatsApp.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        string claveRecibida = Request.Headers["X-WhatsApp-Key"].ToString();
        if (string.IsNullOrWhiteSpace(claveRecibida) ||
            !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(claveEsperada),
                Encoding.UTF8.GetBytes(claveRecibida)))
        {
            return Unauthorized();
        }

        try
        {
            return Ok(await repositorio.CrearAsync(request, cancellationToken));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensaje = exception.Message });
        }
        catch (CitaWhatsAppConflictoException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
    }
}