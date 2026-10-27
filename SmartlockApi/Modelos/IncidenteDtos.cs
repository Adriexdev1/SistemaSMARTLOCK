using System.ComponentModel.DataAnnotations;

namespace SmartlockApi.Modelos;

/// <summary>Incidente almacenado en el sistema.</summary>
public sealed record IncidenteResponse(
    int IdIncidente,
    string Tipo,
    string Descripcion,
    string Prioridad,
    string Estado,
    string Fecha,
    string Hora,
    string? UsuarioRegistro);

/// <summary>Datos para registrar un incidente.</summary>
public sealed record CrearIncidenteRequest
{
    [Required, MaxLength(80)]
    public required string Tipo { get; init; }

    [Required, MaxLength(500)]
    public required string Descripcion { get; init; }

    [Required, RegularExpression("^(Crítica|Alta|Media|Baja)$")]
    public required string Prioridad { get; init; }

    [Range(1, int.MaxValue)]
    public required int IdUsuarioRegistro { get; init; }
}

/// <summary>Nuevo estado de un incidente.</summary>
public sealed record ActualizarEstadoIncidenteRequest
{
    [Required, RegularExpression("^(Pendiente|En progreso|Resuelto)$")]
    public required string Estado { get; init; }
}