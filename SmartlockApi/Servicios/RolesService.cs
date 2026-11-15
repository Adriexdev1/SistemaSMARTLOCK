using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class RolesService(RolesRepositorio repositorio) : IRolesService
{
    public Task<IReadOnlyList<RolResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);

    public Task<RolResponse?> ObtenerAsync(int idRol, CancellationToken cancellationToken) =>
        repositorio.ObtenerAsync(idRol, cancellationToken);

    public async Task<RolResponse?> CrearAsync(CrearRolRequest request, CancellationToken cancellationToken)
    {
        int idRol = await repositorio.CrearAsync(request, cancellationToken);
        return await repositorio.ObtenerAsync(idRol, cancellationToken);
    }

    public Task<bool> ActualizarAsync(int idRol, ActualizarRolRequest request, CancellationToken cancellationToken) =>
        repositorio.ActualizarAsync(idRol, request, cancellationToken);

    public Task<bool> EliminarAsync(int idRol, CancellationToken cancellationToken) =>
        repositorio.EliminarAsync(idRol, cancellationToken);
}