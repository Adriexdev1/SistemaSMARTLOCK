using SmartlockApi.Datos;

namespace SmartlockApi.Servicios;

public sealed class EventosExpiracionWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<EventosExpiracionWorker> logger) : BackgroundService
{
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Intervalo);

        do
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var repositorio = scope.ServiceProvider.GetRequiredService<EventosRepositorioMulti>();
                int desactivados = await repositorio.DesactivarVencidosAsync(stoppingToken);

                if (desactivados > 0)
                    logger.LogInformation("Se desactivaron {Cantidad} eventos vencidos.", desactivados);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "No se pudieron desactivar los eventos vencidos.");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
