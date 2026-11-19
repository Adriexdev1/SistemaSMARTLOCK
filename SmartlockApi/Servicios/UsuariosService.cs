using SmartlockApi.Datos;
using SmartlockApi.Modelos;

namespace SmartlockApi.Servicios;

public sealed class UsuariosService(UsuariosRepositorio repositorio) : IUsuariosService
{
    public Task<IReadOnlyList<UsuarioResponse>> ListarAsync(CancellationToken cancellationToken) =>
        repositorio.ListarAsync(cancellationToken);

    public Task<UsuarioResponse?> ObtenerAsync(int idUsuario, CancellationToken cancellationToken) =>
        repositorio.ObtenerAsync(idUsuario, cancellationToken);

    public Task<IReadOnlyList<RolUsuarioResponse>> ListarRolesAsync(CancellationToken cancellationToken) =>
        repositorio.ListarRolesAsync(cancellationToken);

    public async Task<UsuarioResponse?> CrearAsync(CrearUsuarioRequest request, CancellationToken cancellationToken)
    {
        int idUsuario = await repositorio.CrearAsync(
            request,
            PasswordHasher.Hash(request.Contrasena),
            cancellationToken);

        return await repositorio.ObtenerAsync(idUsuario, cancellationToken);
    }

    public Task<bool> ActualizarAsync(
        int idUsuario,
        ActualizarUsuarioRequest request,
        CancellationToken cancellationToken) =>
        repositorio.ActualizarAsync(idUsuario, request, cancellationToken);

    public Task<bool> EliminarAsync(int idUsuario, CancellationToken cancellationToken) =>
        repositorio.EliminarAsync(idUsuario, cancellationToken);
}