using Microsoft.Data.SqlClient;
using System.Data;

namespace SmartlockApi.Datos
{

    public class AccesoRepositorio
    {
        private readonly string _cadenaConexion;

        public AccesoRepositorio(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("SmartlockDb")
                ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");
        }

        public async Task<string> GenerarQrAsync(int idDetalleEvento)
        {
            await using var conn = new SqlConnection(_cadenaConexion);
            await using var cmd = new SqlCommand("SP_GENERAR_QR_DETALLE", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@id_detalle_evento", idDetalleEvento);
            var pCodigo = cmd.Parameters.Add("@codigo_qr", SqlDbType.VarChar, 100);
            pCodigo.Direction = ParameterDirection.Output;

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (string)pCodigo.Value!;
        }

        public async Task<(bool valido, int idUsuario, string nombreUsuario, string nombreEvento)> ValidarQrAsync(string codigoQr)
        {
            await using var conn = new SqlConnection(_cadenaConexion);
            await using var cmd = new SqlCommand("SP_VALIDAR_QR", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@codigo_qr", codigoQr);

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                int idUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                string nombreUsuario = reader.GetString(reader.GetOrdinal("nombre_usuario"));
                string nombreEvento = reader.GetString(reader.GetOrdinal("nombre_evento"));
                return (true, idUsuario, nombreUsuario, nombreEvento);
            }

            return (false, 0, string.Empty, string.Empty);
        }


        public async Task<bool> ConsumirQrAsync(string codigoQr)
        {
            await using var conn = new SqlConnection(_cadenaConexion);
            await using var cmd = new SqlCommand("SP_CONSUMIR_QR", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@codigo_qr", codigoQr);

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync();
        }

        public async Task<string> DiagnosticarQrAsync(string codigoQr)
        {
            await using var conn = new SqlConnection(_cadenaConexion);
            await using var cmd = new SqlCommand("SP_DIAGNOSTICO_QR", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@codigo_qr", codigoQr);

            await conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return reader.GetString(reader.GetOrdinal("motivo"));

            return "QR_INEXISTENTE";
        }

        public async Task<string?> ObtenerPinHashAsync(int idUsuario)
        {
            await using var conn = new SqlConnection(_cadenaConexion);
            await using var cmd = new SqlCommand("SP_OBTENER_PIN_USUARIO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

            await conn.OpenAsync();
            var resultado = await cmd.ExecuteScalarAsync();
            return resultado as string;
        }
    }
}
