using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IRolesService
{
    Task<IReadOnlyList<RolResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<RolResponse?> ObtenerAsync(int idRol, CancellationToken cancellationToken);
    Task<RolResponse?> CrearAsync(CrearRolRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int idRol, ActualizarRolRequest request, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int idRol, CancellationToken cancellationToken);
}