using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

public sealed record CrearCitaWhatsAppRequest
{
    [Required, Phone, MaxLength(25)]
    public required string TelefonoWhatsApp { get; init; }

    [Required, EmailAddress, MaxLength(100)]
    public required string CorreoUsuario { get; init; }

    [MaxLength(200)]
    public string? NombreUsuario { get; init; }

    public DateOnly? FechaNacimiento { get; init; }

    [Required, MaxLength(150)]
    public required string Motivo { get; init; }

    [Required]
    public DateOnly? Fecha { get; init; }

    [Required]
    public TimeOnly? HoraInicio { get; init; }

    [Required]
    public TimeOnly? HoraFin { get; init; }
}

public sealed record CrearCitaWhatsAppResponse(
    int IdUsuario,
    int IdEvento,
    int IdDetalleEvento,
    string CodigoQr,
    DateOnly Fecha,
    TimeOnly HoraInicio,
    TimeOnly HoraFin
);