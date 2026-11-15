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
    string HoraFin,
    string Estado,
    IReadOnlyList<ParticipanteEventoResponse> Participantes);

/// <summary>Datos para programar un acceso para un usuario existente.</summary>
public sealed record CrearEventoRequest
{
    [Required, MinLength(1)]
    public required int[] IdUsuarios { get; init; }

    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    public required DateOnly Fecha { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    public required TimeOnly HoraFin { get; init; }
}

/// <summary>Datos para modificar un acceso programado.</summary>
public sealed record ActualizarEventoRequest
{
    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    public required DateOnly Fecha { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    public required TimeOnly HoraFin { get; init; }

    [Required, RegularExpression("^(Activo|Completado)$")]
    public required string Estado { get; init; }
}

/// <summary>Usuario que se agregará a un evento.</summary>
public sealed record AgregarParticipanteRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }
}