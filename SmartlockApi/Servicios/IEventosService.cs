using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IEventosService
{
    Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<EventoResponse?> ObtenerAsync(int idEvento, CancellationToken cancellationToken);
    Task<EventoResponse?> CrearAsync(CrearEventoRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int idEvento, ActualizarEventoRequest request, CancellationToken cancellationToken);
    Task<(bool EventoExiste, bool Agregado)> AgregarParticipanteAsync(int idEvento, int idUsuario, CancellationToken cancellationToken);
    Task<bool> EliminarParticipanteAsync(int idEvento, int idDetalleEvento, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int idEvento, CancellationToken cancellationToken);
}