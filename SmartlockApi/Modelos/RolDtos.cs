using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Rol de acceso configurado en el sistema.</summary>
public sealed record RolResponse(
    int IdRol,
    string NombreRol,
    string? DescripcionRol,
    int PermisoRol,
    int EstadoRol,
    DateTimeOffset? FechaRol);

/// <summary>Datos para crear un rol.</summary>
public sealed record CrearRolRequest
{
    [Required, MaxLength(50)]
    public required string NombreRol { get; init; }

    [MaxLength(150)]
    public string? DescripcionRol { get; init; }

    [Range(0, 2)]
    public required int PermisoRol { get; init; }

    [Range(0, 1)]
    public required int EstadoRol { get; init; }
}

/// <summary>Datos para editar un rol existente.</summary>
public sealed record ActualizarRolRequest
{
    [Required, MaxLength(50)]
    public required string NombreRol { get; init; }

    [MaxLength(150)]
    public string? DescripcionRol { get; init; }

    [Range(0, 2)]
    public required int PermisoRol { get; init; }

    [Range(0, 1)]
    public required int EstadoRol { get; init; }
}