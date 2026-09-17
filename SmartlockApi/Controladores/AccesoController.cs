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
        private readonly ILogger<AccesoController> _logger;

        public AccesoController(AccesoRepositorio repo, ILogger<AccesoController> logger)
        {
            _repo = repo;
            _logger = logger;
        }


        [HttpPost("generar-qr")]
        public async Task<ActionResult<GenerarQrResponse>> GenerarQr([FromBody] GenerarQrRequest request)
        {
            var codigo = await _repo.GenerarQrAsync(request.IdDetalleEvento);
            return Ok(new GenerarQrResponse { CodigoQr = codigo });
        }

        // Endpoint TEMPORAL solo para pruebas genera el hash de un PIN
        [HttpGet("hash-pin/{pin}")]
        public ActionResult<string> HashPin(string pin)
        {
            return Ok(PasswordHasher.Hash(pin));
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

            string? pinHash = await _repo.ObtenerPinHashAsync(idUsuario);

            if (!string.IsNullOrEmpty(pinHash))
            {
                if (string.IsNullOrWhiteSpace(request.Pin) ||
                    !PasswordHasher.Verificar(request.Pin, pinHash))
                {
                    _logger.LogWarning("Acceso rechazado por PIN incorrecto. Usuario={Usuario} QR={Qr}", idUsuario, request.CodigoQr);

                    return Ok(new ValidarAccesoResponse
                    {
                        Autorizado = false,
                        Resultado = ResultadoAcceso.PinIncorrecto,
                        NombreUsuario = nombreUsuario,
                        NombreEvento = nombreEvento
                    });
                }
            }

            bool consumido = await _repo.ConsumirQrAsync(request.CodigoQr);

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
    }
}
