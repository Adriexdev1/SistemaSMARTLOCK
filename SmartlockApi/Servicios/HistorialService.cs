using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class HistorialService(HistorialRepositorio repositorio) : IHistorialService
{
    public Task<IReadOnlyList<HistorialResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);
}