using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class EventosService(EventosRepositorio repositorio) : IEventosService
{
    public Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);

    public Task<EventoResponse?> ObtenerAsync(int idDetalleEvento, CancellationToken cancellationToken) =>
        repositorio.ObtenerAsync(idDetalleEvento, cancellationToken);

    public async Task<EventoResponse?> CrearAsync(CrearEventoRequest request, CancellationToken cancellationToken)
    {
        int idDetalleEvento = await repositorio.CrearAsync(request, cancellationToken);
        return await repositorio.ObtenerAsync(idDetalleEvento, cancellationToken);
    }

    public Task<bool> ActualizarAsync(int idDetalleEvento, ActualizarEventoRequest request, CancellationToken cancellationToken) =>
        repositorio.ActualizarAsync(idDetalleEvento, request, cancellationToken);

    public Task<bool> EliminarAsync(int idDetalleEvento, CancellationToken cancellationToken) =>
        repositorio.EliminarAsync(idDetalleEvento, cancellationToken);
}