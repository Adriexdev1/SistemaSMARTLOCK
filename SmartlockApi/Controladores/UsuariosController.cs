using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(IUsuariosService usuariosService) : ControllerBase
{
    /// <summary>Lista usuarios sin exponer sus contraseñas.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<UsuarioResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UsuarioResponse>>> Listar(CancellationToken cancellationToken) =>
        Ok(await usuariosService.ListarAsync(cancellationToken));

    /// <summary>Lista los roles disponibles para asignar.</summary>
    [HttpGet("roles")]
    [ProducesResponseType<IReadOnlyList<RolUsuarioResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RolUsuarioResponse>>> ListarRoles(CancellationToken cancellationToken) =>
        Ok(await usuariosService.ListarRolesAsync(cancellationToken));

    /// <summary>Obtiene un usuario por su identificador.</summary>
    [HttpGet("{idUsuario:int}")]
    [ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioResponse>> Obtener(int idUsuario, CancellationToken cancellationToken)
    {
        UsuarioResponse? usuario = await usuariosService.ObtenerAsync(idUsuario, cancellationToken);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Crea un usuario activo y almacena su contraseña como hash.</summary>
    [HttpPost]
    [ProducesResponseType<UsuarioResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioResponse>> Crear(
        [FromBody] CrearUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            UsuarioResponse? usuario = await usuariosService.CrearAsync(request, cancellationToken);
            return usuario is null
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : CreatedAtAction(nameof(Obtener), new { idUsuario = usuario.IdUsuario }, usuario);
        }
        catch (SqlException exception) when (exception.Number is 2601 or 2627)
        {
            return Conflict(new { mensaje = "Ese correo ya está registrado." });
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El rol seleccionado no existe." });
        }
    }

    /// <summary>Actualiza nombre, correo, rol y estado de un usuario.</summary>
    [HttpPut("{idUsuario:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Actualizar(
        int idUsuario,
        [FromBody] ActualizarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            bool actualizado = await usuariosService.ActualizarAsync(idUsuario, request, cancellationToken);
            return actualizado ? NoContent() : NotFound();
        }
        catch (SqlException exception) when (exception.Number is 2601 or 2627)
        {
            return Conflict(new { mensaje = "Ese correo ya está registrado." });
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return BadRequest(new { mensaje = "El rol seleccionado no existe." });
        }
    }

    /// <summary>Elimina un usuario que no tenga relaciones activas.</summary>
    [HttpDelete("{idUsuario:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int idUsuario, CancellationToken cancellationToken)
    {
        try
        {
            bool eliminado = await usuariosService.EliminarAsync(idUsuario, cancellationToken);
            return eliminado ? NoContent() : NotFound();
        }
        catch (SqlException exception) when (exception.Number == 547)
        {
            return Conflict(new { mensaje = "No se puede eliminar: el usuario tiene eventos o registros relacionados." });
        }
    }
}