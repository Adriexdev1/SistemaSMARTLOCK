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
    bool TieneTarjetaRfidAsignada,
    int? PermisoRol);

/// <summary>Datos para vincular una tarjeta RFID al usuario.</summary>
public sealed record ConfigurarTarjetaRfidRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }

    [Required, MaxLength(50)]
    public required string Uid { get; init; }
}

/// <summary>Datos para quitar la tarjeta RFID vinculada.</summary>
public sealed record EliminarTarjetaRfidRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string ContrasenaActual { get; init; }
}

/// <summary>Indica si el usuario tiene una tarjeta RFID vinculada.</summary>
public sealed record EstadoTarjetaRfidResponse(bool TieneTarjetaRfidAsignada);

public enum ResultadoConfigurarTarjetaRfid
{
    Configurada,
    CredencialesInvalidas,
    UidInvalido,
    UsuarioNoEncontrado,
    SinPermiso,
    ServicioNoConfigurado
}

public enum ResultadoEliminarTarjetaRfid
{
    Eliminada,
    CredencialesInvalidas,
    UsuarioNoEncontrado,
    SinPermiso
}

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