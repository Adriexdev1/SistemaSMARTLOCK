using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Credenciales requeridas para iniciar sesión.</summary>
public sealed record LoginRequest
{
    [Required, EmailAddress, MaxLength(100)]
    public required string Correo { get; init; }

    [Required, MaxLength(100)]
    public required string Contrasena { get; init; }
}

/// <summary>Datos públicos del usuario que inició sesión.</summary>
public sealed record LoginResponse(
    int IdUsuario,
    string NombreUsuario,
    string CorreoUsuario,
    string? TelefonoUsuario,
    string? Rol,
    bool TienePinConfigurado);

/// <summary>Datos para configurar o cambiar el PIN personal.</summary>
public sealed record ConfigurarPinRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }

    [Required, RegularExpression(@"^\d{4,8}$")]
    public required string Pin { get; init; }
}

/// <summary>Datos para quitar el PIN personal.</summary>
public sealed record EliminarPinRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }
}

/// <summary>Indica si el usuario tiene un PIN configurado.</summary>
public sealed record EstadoPinResponse(bool TienePinConfigurado);

/// <summary>Datos necesarios para cambiar la contraseña de un usuario.</summary>
public sealed record CambiarContrasenaRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }

    [Required, MinLength(8), MaxLength(100)]
    public required string NuevaContrasena { get; init; }
}

/// <summary>Datos para actualizar la información personal de un usuario.</summary>
public sealed record ActualizarPerfilRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(200)]
    public required string NombreUsuario { get; init; }

    [Required, EmailAddress, MaxLength(100)]
    public required string CorreoUsuario { get; init; }

    [MaxLength(25)]
    public string? TelefonoUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }
}

public enum ResultadoActualizarPerfil
{
    Actualizado,
    CredencialesInvalidas,
    UsuarioNoEncontrado,
    CorreoEnUso
}