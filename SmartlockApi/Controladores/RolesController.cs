using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/roles")]
public sealed class RolesController(IRolesService rolesService) : ControllerBase
{
    /// <summary>Lista los roles configurados.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<RolResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RolResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await rolesService.ListarAsync(cancellationToken));

    /// <summary>Obtiene un rol por identificador.</summary>
    [HttpGet("{idRol:int}")]
    [ProducesResponseType<RolResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RolResponse>> Obtener(int idRol, CancellationToken cancellationToken)
    {
        RolResponse? rol = await rolesService.ObtenerAsync(idRol, cancellationToken);
        return rol is null ? NotFound() : Ok(rol);
    }

    /// <summary>Crea un rol.</summary>
    [HttpPost]
    [ProducesResponseType<RolResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RolResponse>> Crear(
        [FromBody] CrearRolRequest request,
        CancellationToken cancellationToken)
    {
        RolResponse? rol = await rolesService.CrearAsync(request, cancellationToken);
        return rol is null
            ? StatusCode(StatusCodes.Status500InternalServerError)
            : CreatedAtAction(nameof(Obtener), new { idRol = rol.IdRol }, rol);
    }

    /// <summary>Actualiza nombre, descripción, permiso y estado.</summary>
    [HttpPut("{idRol:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int idRol,
        [FromBody] ActualizarRolRequest request,
        CancellationToken cancellationToken)
    {
        bool actualizado = await rolesService.ActualizarAsync(idRol, request, cancellationToken);
        return actualizado ? NoContent() : NotFound();
    }

    /// <summary>Elimina un rol que no esté asignado a usuarios.</summary>
    [HttpDelete("{idRol:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int idRol, CancellationToken cancellationToken)
    {
        try
        {
            bool eliminado = await rolesService.EliminarAsync(idRol, cancellationToken);
            return eliminado ? NoContent() : NotFound();
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return Conflict(new { mensaje = "No se puede eliminar un rol asignado a usuarios." });
        }
    }
}