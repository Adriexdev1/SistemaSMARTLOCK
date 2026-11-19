using System;
using System.Data.SqlClient;

public partial class puerta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        string token = Request.QueryString["token"];

        if (string.IsNullOrEmpty(id) || token != "ABC123")
        {
            Response.Write("Acceso denegado");
            return;
        }

        try
        {
            string connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["ConexionSmartLock"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryValidacion = @"
                SELECT 
                    e.fechaprogramada_evento,
                    e.fechalimite_evento
                FROM detalle_eventos de
                INNER JOIN eventos e 
                    ON e.id_evento = de.id_evento
                WHERE de.id_detalle_evento = @id";

                DateTime fechaInicio;
                DateTime fechaFin;

                using (SqlCommand cmdValidar = new SqlCommand(queryValidacion, conn))
                {
                    cmdValidar.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader dr = cmdValidar.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            Response.Write("Evento no encontrado");
                            return;
                        }

                        fechaInicio = Convert.ToDateTime(dr["fechaprogramada_evento"]);
                        fechaFin = Convert.ToDateTime(dr["fechalimite_evento"]);
                    }
                }

                DateTime fechaActualMasUnaHora = DateTime.Now.AddHours(-1);



                if (fechaActualMasUnaHora >= fechaInicio &&
                    fechaActualMasUnaHora <= fechaFin)
                {
                    string queryUpdate = @"
                    UPDATE detalle_eventos
                    SET acceso_detalle_evento = DATEADD(HOUR, -1, GETDATE())
                    WHERE id_detalle_evento = @id";

                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@id", id);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    Response.Write("OK");
                }
                else
                {
                    Response.Write("Fuera del horario permitido");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }
    }
}