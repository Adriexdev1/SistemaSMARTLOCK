using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IIncidentesService
{
    Task<IReadOnlyList<IncidenteResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<IncidenteResponse?> ObtenerAsync(int idIncidente, CancellationToken cancellationToken);
    Task<IncidenteResponse?> CrearAsync(CrearIncidenteRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarEstadoAsync(int idIncidente, string estado, CancellationToken cancellationToken);
}