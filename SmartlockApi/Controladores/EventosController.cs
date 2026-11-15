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

    /// <summary>Obtiene un evento con todos sus participantes.</summary>
    [HttpGet("{idEvento:int}")]
    [ProducesResponseType<EventoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventoResponse>> Obtener(int idEvento, CancellationToken cancellationToken)
    {
        EventoResponse? evento = await eventosService.ObtenerAsync(idEvento, cancellationToken);
        return evento is null ? NotFound() : Ok(evento);
    }

    /// <summary>Programa un evento y asigna varios usuarios en una transacción.</summary>
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
                : CreatedAtAction(nameof(Obtener), new { idEvento = evento.IdEvento }, evento);
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "Uno o más usuarios seleccionados no existen." });
        }
    }

    /// <summary>Actualiza el horario, tipo y estado de un evento.</summary>
    [HttpPut("{idEvento:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Actualizar(
        int idEvento,
        [FromBody] ActualizarEventoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            bool actualizado = await eventosService.ActualizarAsync(idEvento, request, cancellationToken);
            return actualizado ? NoContent() : NotFound();
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "No se pudo actualizar el evento." });
        }
    }

    /// <summary>Agrega un usuario existente a un evento.</summary>
    [HttpPost("{idEvento:int}/participantes")]
    [ProducesResponseType<EventoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventoResponse>> AgregarParticipante(
        int idEvento,
        [FromBody] AgregarParticipanteRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await eventosService.AgregarParticipanteAsync(idEvento, request.IdUsuario, cancellationToken);
            if (!resultado.EventoExiste) return NotFound();
            if (!resultado.Agregado) return Conflict(new { mensaje = "Ese usuario ya está asignado al evento." });
            return Ok(await eventosService.ObtenerAsync(idEvento, cancellationToken));
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El usuario seleccionado no existe." });
        }
    }

    /// <summary>Quita un participante del evento.</summary>
    [HttpDelete("{idEvento:int}/participantes/{idDetalleEvento:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarParticipante(
        int idEvento,
        int idDetalleEvento,
        CancellationToken cancellationToken) =>
        await eventosService.EliminarParticipanteAsync(idEvento, idDetalleEvento, cancellationToken)
            ? NoContent()
            : NotFound();

    /// <summary>Elimina el evento y sus asignaciones.</summary>
    [HttpDelete("{idEvento:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int idEvento, CancellationToken cancellationToken) =>
        await eventosService.EliminarAsync(idEvento, cancellationToken) ? NoContent() : NotFound();
}