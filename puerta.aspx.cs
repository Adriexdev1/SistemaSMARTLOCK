using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web.UI;

public partial class puerta : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PrepararRespuesta();

        if (!String.Equals(Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
        {
            EscribirRespuesta(405, false);
            return;
        }

        string idTexto = ObtenerParametroUnico("id");
        string codigoQr = ObtenerParametroUnico("token");
        int idDetalleEvento;
        Guid token;

        if (!Int32.TryParse(idTexto, NumberStyles.None, CultureInfo.InvariantCulture, out idDetalleEvento) ||
            idDetalleEvento <= 0 ||
            !Guid.TryParseExact(codigoQr, "D", out token))
        {
            EscribirRespuesta(403, false);
            return;
        }

        codigoQr = token.ToString("D");

        try
        {
            ConnectionStringSettings connectionStringSettings =
                ConfigurationManager.ConnectionStrings["ConexionSmartLock"];
            string connectionString = connectionStringSettings == null
                ? null
                : connectionStringSettings.ConnectionString;

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                System.Diagnostics.Trace.TraceError("QR validation is not configured: ConexionSmartLock is missing.");
                EscribirRespuesta(503, false);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int idDetalleValidado;
                int idUsuario;

                using (SqlCommand command = new SqlCommand("dbo.SP_VALIDAR_QR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 8;
                    command.Parameters.Add("@codigo_qr", SqlDbType.VarChar, 100).Value = codigoQr;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            EscribirRespuesta(403, false);
                            return;
                        }

                        idDetalleValidado = reader.GetInt32(reader.GetOrdinal("id_detalle_evento"));
                        idUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario"));
                    }
                }

                if (idDetalleValidado != idDetalleEvento || !EventoIniciado(connection, idDetalleEvento, codigoQr))
                {
                    EscribirRespuesta(403, false);
                    return;
                }

                if (!UsuarioActivo(connection, idUsuario))
                {
                    EscribirRespuesta(403, false);
                    return;
                }

                using (SqlCommand command = new SqlCommand("dbo.SP_CONSUMIR_QR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 8;
                    command.Parameters.Add("@codigo_qr", SqlDbType.VarChar, 100).Value = codigoQr;
                    command.Parameters.Add("@metodo_acceso", SqlDbType.VarChar, 20).Value = "Solo QR";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read() ||
                            reader.GetInt32(reader.GetOrdinal("id_detalle_evento")) != idDetalleEvento)
                        {
                            EscribirRespuesta(403, false);
                            return;
                        }
                    }
                }
            }

            EscribirRespuesta(200, true);
        }
        catch (SqlException exception)
        {
            System.Diagnostics.Trace.TraceError("QR validation database failure. SQL error number: {0}", exception.Number);
            EscribirRespuesta(503, false);
        }
        catch (Exception exception)
        {
            System.Diagnostics.Trace.TraceError("QR validation failed. Exception type: {0}", exception.GetType().FullName);
            EscribirRespuesta(503, false);
        }
    }

    private string ObtenerParametroUnico(string nombre)
    {
        string[] valores = Request.QueryString.GetValues(nombre);
        return valores != null && valores.Length == 1 ? valores[0] : null;
    }

    private bool EventoIniciado(SqlConnection connection, int idDetalleEvento, string codigoQr)
    {
        const string query = @"
            SELECT CASE
                WHEN e.fechaprogramada_evento IS NULL
                  OR e.fechaprogramada_evento <= GETDATE() THEN 1
                ELSE 0
            END
            FROM dbo.detalle_eventos de
            INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
            WHERE de.id_detalle_evento = @id_detalle_evento
              AND de.codigo_qr = @codigo_qr
              AND de.qr_usado = 0;";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.CommandTimeout = 8;
            command.Parameters.Add("@id_detalle_evento", SqlDbType.Int).Value = idDetalleEvento;
            command.Parameters.Add("@codigo_qr", SqlDbType.VarChar, 100).Value = codigoQr;

            object resultado = command.ExecuteScalar();
            return resultado != null &&
                resultado != DBNull.Value &&
                Convert.ToInt32(resultado, CultureInfo.InvariantCulture) == 1;
        }
    }

    private bool UsuarioActivo(SqlConnection connection, int idUsuario)
    {
        const string query = @"
            SELECT estado_usuario
            FROM dbo.usuarios
            WHERE id_usuario = @id_usuario;";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.CommandTimeout = 8;
            command.Parameters.Add("@id_usuario", SqlDbType.Int).Value = idUsuario;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                return reader.Read() &&
                    reader.GetInt32(reader.GetOrdinal("estado_usuario")) == 1;
            }
        }
    }

    private void PrepararRespuesta()
    {
        Response.Clear();
        Response.ContentType = "application/json";
        Response.ContentEncoding = Encoding.UTF8;
        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.TrySkipIisCustomErrors = true;
    }

    private void EscribirRespuesta(int statusCode, bool autorizado)
    {
        Response.StatusCode = statusCode;
        Response.Write(autorizado ? "{\"acceso\":true}" : "{\"acceso\":false}");
        Context.ApplicationInstance.CompleteRequest();
    }
}
