using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IAuthService
{
    Task<LoginResponse?> AutenticarAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<ResultadoConfigurarTarjetaRfid> ConfigurarTarjetaRfidAsync(
        ConfigurarTarjetaRfidRequest request,
        CancellationToken cancellationToken);
    Task<ResultadoEliminarTarjetaRfid> EliminarTarjetaRfidAsync(
        EliminarTarjetaRfidRequest request,
        CancellationToken cancellationToken);
    Task<bool> CambiarContrasenaAsync(CambiarContrasenaRequest request, CancellationToken cancellationToken);
    Task<ResultadoActualizarPerfil> ActualizarPerfilAsync(ActualizarPerfilRequest request, CancellationToken cancellationToken);
}