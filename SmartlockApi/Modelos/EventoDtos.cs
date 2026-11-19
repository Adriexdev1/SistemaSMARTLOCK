using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Registro de un acceso realizado.</summary>
public sealed record RegistroAccesoResponse(int Id, string Fecha, string Hora, string Resultado);

/// <summary>Participante asignado al evento con su QR individual.</summary>
public sealed record ParticipanteEventoResponse(
    int IdDetalleEvento,
    int IdUsuario,
    string NombreUsuario,
    string EstadoQr,
    string? CodigoQr,
    IReadOnlyList<RegistroAccesoResponse> Historial);

/// <summary>Evento programado con todos sus participantes.</summary>
public sealed record EventoResponse(
    int IdEvento,
    string Tipo,
    string Fecha,
    string Hora,
    string FechaFin,
    string HoraFin,
    string Estado,
    IReadOnlyList<ParticipanteEventoResponse> Participantes);

/// <summary>Datos para programar un acceso para un usuario existente.</summary>
public sealed record CrearEventoRequest : IValidatableObject
{
    [Required, MinLength(1)]
    public required int[] IdUsuarios { get; init; }

    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    [Required]
    public required DateOnly FechaInicio { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    [Required]
    public required DateOnly FechaFin { get; init; }
    public required TimeOnly HoraFin { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaInicio == default || FechaFin == default)
        {
            yield return new ValidationResult(
                "Las fechas de inicio y terminación son obligatorias.",
                [nameof(FechaInicio), nameof(FechaFin)]);
            yield break;
        }

        if (FechaFin.ToDateTime(HoraFin) < FechaInicio.ToDateTime(HoraInicio))
        {
            yield return new ValidationResult(
                "La fecha y hora de terminación no pueden ser anteriores al inicio.",
                [nameof(FechaFin), nameof(HoraFin)]);
        }
    }
}

/// <summary>Datos para modificar un acceso programado.</summary>
public sealed record ActualizarEventoRequest : IValidatableObject
{
    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    [Required]
    public required DateOnly FechaInicio { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    [Required]
    public required DateOnly FechaFin { get; init; }
    public required TimeOnly HoraFin { get; init; }

    [Required, RegularExpression("^(Activo|Inactivo)$")]
    public required string Estado { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaInicio == default || FechaFin == default)
        {
            yield return new ValidationResult(
                "Las fechas de inicio y terminación son obligatorias.",
                [nameof(FechaInicio), nameof(FechaFin)]);
            yield break;
        }

        if (FechaFin.ToDateTime(HoraFin) < FechaInicio.ToDateTime(HoraInicio))
        {
            yield return new ValidationResult(
                "La fecha y hora de terminación no pueden ser anteriores al inicio.",
                [nameof(FechaFin), nameof(HoraFin)]);
        }
    }
}

/// <summary>Usuario que se agregará a un evento.</summary>
public sealed record AgregarParticipanteRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }
}