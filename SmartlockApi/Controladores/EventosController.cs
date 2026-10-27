using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/eventos")]
public sealed class EventosController(IEventosService eventosService) : ControllerBase
{
    /// <summary>Lista accesos programados.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<EventoResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EventoResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await eventosService.ListarAsync(cancellationToken));

    /// <summary>Obtiene un acceso por identificador del detalle.</summary>
    [HttpGet("{idDetalleEvento:int}")]
    [ProducesResponseType<EventoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventoResponse>> Obtener(int idDetalleEvento, CancellationToken cancellationToken)
    {
        EventoResponse? evento = await eventosService.ObtenerAsync(idDetalleEvento, cancellationToken);
        return evento is null ? NotFound() : Ok(evento);
    }

    /// <summary>Programa un acceso para un usuario existente.</summary>
    [HttpPost]
    [ProducesResponseType<EventoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventoResponse>> Crear(
        [FromBody] CrearEventoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            EventoResponse? evento = await eventosService.CrearAsync(request, cancellationToken);
            return evento is null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : CreatedAtAction(nameof(Obtener), new { idDetalleEvento = evento.IdDetalleEvento }, evento);
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El usuario seleccionado no existe." });
        }
    }

    /// <summary>Actualiza participante, horario, tipo y estado de un acceso.</summary>
    [HttpPut("{idDetalleEvento:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Actualizar(
        int idDetalleEvento,
        [FromBody] ActualizarEventoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            bool actualizado = await eventosService.ActualizarAsync(idDetalleEvento, request, cancellationToken);
            return actualizado ? NoContent() : NotFound();
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El usuario seleccionado no existe." });
        }
    }

    /// <summary>Elimina la relación del acceso y el evento si queda sin participantes.</summary>
    [HttpDelete("{idDetalleEvento:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int idDetalleEvento, CancellationToken cancellationToken) =>
        await eventosService.EliminarAsync(idDetalleEvento, cancellationToken) ? NoContent() : NotFound();
}