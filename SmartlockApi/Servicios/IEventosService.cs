using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IEventosService
{
    Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<EventoResponse?> ObtenerAsync(int idDetalleEvento, CancellationToken cancellationToken);
    Task<EventoResponse?> CrearAsync(CrearEventoRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int idDetalleEvento, ActualizarEventoRequest request, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int idDetalleEvento, CancellationToken cancellationToken);
}