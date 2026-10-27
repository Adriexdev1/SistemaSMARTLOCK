using Microsoft.AspNetCore.Mvc;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/historial")]
public sealed class HistorialController(IHistorialService historialService) : ControllerBase
{
    /// <summary>Lista accesos completados almacenados en la base.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<HistorialResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<HistorialResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await historialService.ListarAsync(cancellationToken));
}