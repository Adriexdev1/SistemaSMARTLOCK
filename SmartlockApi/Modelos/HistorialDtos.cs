namespace SmartlockApi.Modelos;

/// <summary>Acceso autorizado o rechazado registrado en el sistema.</summary>
public sealed record HistorialResponse(
    long Id,
    string Fecha,
    string Hora,
    string Usuario,
    int IdDetalleEvento,
    int IdEvento,
    string Resultado,
    string MetodoAcceso);