using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class IncidentesService(IncidentesRepositorio repositorio) : IIncidentesService
{
    public Task<IReadOnlyList<IncidenteResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);

    public Task<IncidenteResponse?> ObtenerAsync(int idIncidente, CancellationToken cancellationToken) =>
        repositorio.ObtenerAsync(idIncidente, cancellationToken);

    public async Task<IncidenteResponse?> CrearAsync(CrearIncidenteRequest request, CancellationToken cancellationToken)
    {
        int idIncidente = await repositorio.CrearAsync(request, cancellationToken);
        return await repositorio.ObtenerAsync(idIncidente, cancellationToken);
    }

    public Task<bool> ActualizarEstadoAsync(int idIncidente, string estado, CancellationToken cancellationToken) =>
        repositorio.ActualizarEstadoAsync(idIncidente, estado, cancellationToken);
}