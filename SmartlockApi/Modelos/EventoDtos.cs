using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Registro de un acceso realizado.</summary>
public sealed record RegistroAccesoResponse(int Id, string Fecha, string Hora, string Resultado);

/// <summary>Acceso programado con su participante y estado QR.</summary>
public sealed record EventoResponse(
    int Id,
    int IdDetalleEvento,
    int IdUsuario,
    string Visitante,
    string Tipo,
    string Fecha,
    string Hora,
    string HoraFin,
    string Estado,
    string EstadoQr,
    string? CodigoQr,
    IReadOnlyList<RegistroAccesoResponse> Historial);

/// <summary>Datos para programar un acceso para un usuario existente.</summary>
public sealed record CrearEventoRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    public required DateOnly Fecha { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    public required TimeOnly HoraFin { get; init; }
}

/// <summary>Datos para modificar un acceso programado.</summary>
public sealed record ActualizarEventoRequest
{
    [Range(1, int.MaxValue)]
    public required int IdUsuario { get; init; }

    [Required, MaxLength(100)]
    public required string Tipo { get; init; }

    public required DateOnly Fecha { get; init; }
    public required TimeOnly HoraInicio { get; init; }
    public required TimeOnly HoraFin { get; init; }

    [Required, RegularExpression("^(Activo|Completado)$")]
    public required string Estado { get; init; }
}