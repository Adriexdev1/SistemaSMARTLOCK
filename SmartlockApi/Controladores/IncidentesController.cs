using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/incidentes")]
public sealed class IncidentesController(IIncidentesService incidentesService) : ControllerBase
{
    /// <summary>Lista incidentes recientes.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<IncidenteResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IncidenteResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await incidentesService.ListarAsync(cancellationToken));

    /// <summary>Obtiene un incidente por identificador.</summary>
    [HttpGet("{idIncidente:int}")]
    [ProducesResponseType<IncidenteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidenteResponse>> Obtener(int idIncidente, CancellationToken cancellationToken)
    {
        IncidenteResponse? incidente = await incidentesService.ObtenerAsync(idIncidente, cancellationToken);
        return incidente is null ? NotFound() : Ok(incidente);
    }

    /// <summary>Registra un incidente como pendiente.</summary>
    [HttpPost]
    [ProducesResponseType<IncidenteResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidenteResponse>> Crear(
        [FromBody] CrearIncidenteRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            IncidenteResponse? incidente = await incidentesService.CrearAsync(request, cancellationToken);
            return incidente is null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : CreatedAtAction(nameof(Obtener), new { idIncidente = incidente.IdIncidente }, incidente);
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El usuario que registra el incidente no existe." });
        }
    }

    /// <summary>Actualiza el estado de un incidente.</summary>
    [HttpPatch("{idIncidente:int}/estado")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarEstado(
        int idIncidente,
        [FromBody] ActualizarEstadoIncidenteRequest request,
        CancellationToken cancellationToken)
    {
        bool actualizado = await incidentesService.ActualizarEstadoAsync(idIncidente, request.Estado, cancellationToken);
        return actualizado ? NoContent() : NotFound();
    }
}