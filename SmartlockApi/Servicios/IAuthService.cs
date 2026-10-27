using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IAuthService
{
    Task<LoginResponse?> AutenticarAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<bool> ConfigurarPinAsync(ConfigurarPinRequest request, CancellationToken cancellationToken);
    Task<bool> EliminarPinAsync(EliminarPinRequest request, CancellationToken cancellationToken);
    Task<bool> CambiarContrasenaAsync(CambiarContrasenaRequest request, CancellationToken cancellationToken);
    Task<ResultadoActualizarPerfil> ActualizarPerfilAsync(ActualizarPerfilRequest request, CancellationToken cancellationToken);
}