using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IHistorialService
{
    Task<IReadOnlyList<HistorialResponse>> ListarAsync(CancellationToken cancellationToken);
}