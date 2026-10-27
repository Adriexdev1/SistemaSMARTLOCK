using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public interface IUsuariosService
{
    Task<IReadOnlyList<UsuarioResponse>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioResponse?> ObtenerAsync(int idUsuario, CancellationToken cancellationToken);
    Task<IReadOnlyList<RolUsuarioResponse>> ListarRolesAsync(CancellationToken cancellationToken);
    Task<UsuarioResponse?> CrearAsync(CrearUsuarioRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int idUsuario, ActualizarUsuarioRequest request, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int idUsuario, CancellationToken cancellationToken);
}