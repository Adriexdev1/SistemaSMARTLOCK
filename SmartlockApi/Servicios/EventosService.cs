using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class EventosService(EventosRepositorioMulti repositorio) : IEventosService
{
    private const string MarcadorEventoPermanenteRfid = "SMARTLOCK:RFID:PERMANENT_ACCESS:v1";

    public Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);

    public Task<EventoResponse?> ObtenerAsync(int idEvento, CancellationToken cancellationToken) =>
        repositorio.ObtenerAsync(idEvento, cancellationToken);

    public async Task<EventoResponse?> CrearAsync(CrearEventoRequest request, CancellationToken cancellationToken)
    {
        if (string.Equals(request.Tipo.Trim(), MarcadorEventoPermanenteRfid, StringComparison.OrdinalIgnoreCase))
            return null;

        int idEvento = await repositorio.CrearAsync(request, cancellationToken);
        return await repositorio.ObtenerAsync(idEvento, cancellationToken);
    }

    public Task<bool> ActualizarAsync(int idEvento, ActualizarEventoRequest request, CancellationToken cancellationToken) =>
        string.Equals(request.Tipo.Trim(), MarcadorEventoPermanenteRfid, StringComparison.OrdinalIgnoreCase)
            ? Task.FromResult(false)
            : repositorio.ActualizarAsync(idEvento, request, cancellationToken);

    public async Task<(bool EventoExiste, bool Agregado)> AgregarParticipanteAsync(
        int idEvento,
        int idUsuario,
        CancellationToken cancellationToken)
    {
        if (await repositorio.ObtenerAsync(idEvento, cancellationToken) is null)
            return (false, false);

        int idDetalle = await repositorio.AgregarParticipanteAsync(idEvento, idUsuario, cancellationToken);
        return (true, idDetalle > 0);
    }

    public Task<bool> EliminarParticipanteAsync(int idEvento, int idDetalleEvento, CancellationToken cancellationToken) =>
        repositorio.EliminarParticipanteAsync(idEvento, idDetalleEvento, cancellationToken);

    public Task<bool> EliminarAsync(int idEvento, CancellationToken cancellationToken) =>
        repositorio.EliminarAsync(idEvento, cancellationToken);
}