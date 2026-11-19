using System.ComponentModel.DataAnnotations;

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
    }

    public sealed record ValidarTarjetaRfidRequest
    {
        [Required, MaxLength(50)]
        public required string Uid { get; init; }
    }

    public sealed record ValidarTarjetaRfidResponse(
        bool Autorizado,
        int? IdUsuario,
        string? NombreUsuario,
        string? NombreEvento,
        int? IdDetalleEvento,
        DateTime? FechaAcceso);

    public enum ResultadoAcceso
    {
        Autorizado,
        QrInexistente,
        QrYaUsado,
        QrExpirado,
        EventoInactivo
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
