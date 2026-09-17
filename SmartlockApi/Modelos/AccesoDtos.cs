namespace SmartlockApi.Modelos
{
    public class GenerarQrRequest
    {
        public int IdDetalleEvento { get; set; }
    }

    public class GenerarQrResponse
    {
        public string CodigoQr { get; set; } = string.Empty;
    }
    public class ValidarAccesoRequest
    {
        public string CodigoQr { get; set; } = string.Empty;

        // Opcional: si el establecimiento exige segunda capa (PIN).
        public string? Pin { get; set; }
    }
    public enum ResultadoAcceso
    {
        Autorizado,
        QrInexistente,
        QrYaUsado,
        QrExpirado,
        EventoInactivo,
        PinIncorrecto,
        PinRequeridoNoConfigurado
    }
    public class ValidarAccesoResponse
    {
        public bool Autorizado { get; set; }
        public ResultadoAcceso Resultado { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreEvento { get; set; }
        public DateTime? FechaAcceso { get; set; }
    }
}
