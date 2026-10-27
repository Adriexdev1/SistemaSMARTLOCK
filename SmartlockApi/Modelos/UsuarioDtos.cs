using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Datos públicos de un usuario para administración.</summary>
public sealed record UsuarioResponse(
    int IdUsuario,
    string NombreUsuario,
    string CorreoUsuario,
    string? TelefonoUsuario,
    int? IdRol,
    string? Rol,
    int EstadoUsuario);

/// <summary>Rol seleccionable al crear o editar un usuario.</summary>
public sealed record RolUsuarioResponse(int IdRol, string NombreRol);

/// <summary>Datos necesarios para crear un usuario.</summary>
public sealed record CrearUsuarioRequest
{
    [Required, MaxLength(200)]
    public required string NombreUsuario { get; init; }

    [Required, EmailAddress, MaxLength(100)]
    public required string CorreoUsuario { get; init; }

    [Required, MinLength(8), MaxLength(100)]
    public required string Contrasena { get; init; }

    public int? IdRol { get; init; }
}

/// <summary>Datos modificables de un usuario existente.</summary>
public sealed record ActualizarUsuarioRequest
{
    [Required, MaxLength(200)]
    public required string NombreUsuario { get; init; }

    [Required, EmailAddress, MaxLength(100)]
    public required string CorreoUsuario { get; init; }

    public int? IdRol { get; init; }

    [Range(0, 2)]
    public required int EstadoUsuario { get; init; }
}