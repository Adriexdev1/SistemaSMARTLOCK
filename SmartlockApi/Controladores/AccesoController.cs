using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using SmartlockApi.Datos;
using SmartlockApi.Modelos;
using SmartlockApi.Servicios;

namespace SmartlockApi.Controladores
{
    [ApiController]
    [Route("api/acceso")]
    public class AccesoController : ControllerBase
    {
        private readonly AccesoRepositorio _repo;
        private readonly AccesoRfidRepositorio _rfidRepo;
        private readonly RfidCardHasher _tarjetaHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccesoController> _logger;

        public AccesoController(
            AccesoRepositorio repo,
            AccesoRfidRepositorio rfidRepo,
            RfidCardHasher tarjetaHasher,
            IConfiguration configuration,
            ILogger<AccesoController> logger)
        {
            _repo = repo;
            _rfidRepo = rfidRepo;
            _tarjetaHasher = tarjetaHasher;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("generar-qr")]
        public async Task<ActionResult<GenerarQrResponse>> GenerarQr([FromBody] GenerarQrRequest request)
        {
            var codigo = await _repo.GenerarQrAsync(request.IdDetalleEvento);
            return Ok(new GenerarQrResponse { CodigoQr = codigo });
        }

        [HttpPost("validar")]
        public async Task<ActionResult<ValidarAccesoResponse>> Validar([FromBody] ValidarAccesoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CodigoQr))
                return BadRequest("codigoQr es requerido.");

            var (valido, idUsuario, nombreUsuario, nombreEvento) =
                await _repo.ValidarQrAsync(request.CodigoQr);

            if (!valido)
            {
                string motivo = await _repo.DiagnosticarQrAsync(request.CodigoQr);
                _logger.LogWarning("Acceso rechazado. QR={Qr} Motivo={Motivo}", request.CodigoQr, motivo);

                return Ok(new ValidarAccesoResponse
                {
                    Autorizado = false,
                    Resultado = motivo switch
                    {
                        "QR_YA_USADO" => ResultadoAcceso.QrYaUsado,
                        "QR_EXPIRADO" => ResultadoAcceso.QrExpirado,
                        "EVENTO_INACTIVO" => ResultadoAcceso.EventoInactivo,
                        _ => ResultadoAcceso.QrInexistente
                    }
                });
            }

            bool consumido = await _repo.ConsumirQrAsync(request.CodigoQr, "Solo QR");

            if (!consumido)
            {
                _logger.LogWarning("Acceso rechazado: el QR se consumio en otra solicitud simultanea. QR={Qr}", request.CodigoQr);
                return Ok(new ValidarAccesoResponse
                {
                    Autorizado = false,
                    Resultado = ResultadoAcceso.QrYaUsado,
                    NombreUsuario = nombreUsuario,
                    NombreEvento = nombreEvento
                });
            }

            _logger.LogInformation("Acceso autorizado. Usuario={Usuario} Evento={Evento}", nombreUsuario, nombreEvento);

            return Ok(new ValidarAccesoResponse
            {
                Autorizado = true,
                Resultado = ResultadoAcceso.Autorizado,
                NombreUsuario = nombreUsuario,
                NombreEvento = nombreEvento,
                FechaAcceso = DateTime.Now
            });
        }

        [HttpPost("validar-rfid")]
        [ProducesResponseType<ValidarTarjetaRfidResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<ValidarTarjetaRfidResponse>> ValidarTarjetaRfid(
            [FromBody] ValidarTarjetaRfidRequest request,
            CancellationToken cancellationToken)
        {
            string? claveDispositivo = _configuration["Rfid:DeviceApiKey"]
                ?? Environment.GetEnvironmentVariable("SMARTLOCK_RFID_DEVICE_API_KEY");
            if (string.IsNullOrWhiteSpace(claveDispositivo) || Encoding.UTF8.GetByteCount(claveDispositivo) < 32)
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { mensaje = "La autenticación del lector RFID no está configurada." });

            string claveRecibida = Request.Headers["X-Smartlock-Device-Key"].ToString();
            if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(claveRecibida),
                Encoding.UTF8.GetBytes(claveDispositivo)))
            {
                return Unauthorized();
            }

            if (!_tarjetaHasher.EstaConfigurado)
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { mensaje = "El servicio de tarjetas RFID no está configurado." });

            if (!_tarjetaHasher.TryHashUid(request.Uid, out string hashTarjeta))
                return BadRequest(new { mensaje = "El UID debe contener 4, 7 o 10 bytes en hexadecimal." });

            try
            {
                AccesoRfidRegistrado? registro = await _rfidRepo.RegistrarAsync(hashTarjeta, cancellationToken);
                return Ok(registro is null
                    ? new ValidarTarjetaRfidResponse(false, null, null, null, null, null)
                    : new ValidarTarjetaRfidResponse(
                        true,
                        registro.IdUsuario,
                        registro.NombreUsuario,
                        "Acceso permanente RFID",
                        registro.IdDetalleEvento,
                        registro.FechaAcceso));
            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, "No se pudo registrar el acceso RFID.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { mensaje = "No se pudo validar la tarjeta RFID en la base de datos." });
            }
        }
    }
}
