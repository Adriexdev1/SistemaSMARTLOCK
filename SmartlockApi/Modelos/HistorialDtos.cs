namespace SmartlockApi.Modelos;

/// <summary>Acceso completado registrado en el sistema.</summary>
public sealed record HistorialResponse(
    int Id,
    string Fecha,
    string Hora,
    string Usuario,
    int IdDetalleEvento,
    string Resultado,
    string MetodoAcceso);