using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CD_Evento
{
    public async Task<List<Evento>> Listar()
    {
        List<Evento> lista = new List<Evento>();

        using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
        {
            try
            {
                string query = "SELECT id_evento, nombre_evento, fechaprogramada_evento, fechalimite_evento, descripcion_evento, estado_evento, fecha_evento FROM eventos";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                await oconexion.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Evento()
                        {
                            id_evento = Convert.ToInt32(dr["id_evento"]),
                            nombre_evento = dr["nombre_evento"].ToString(),
                            fechaprogramada_evento = dr["fechaprogramada_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechaprogramada_evento"]),
                            fechalimite_evento = dr["fechalimite_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechalimite_evento"]),
                            descripcion_evento = dr["descripcion_evento"] == DBNull.Value ? "" : dr["descripcion_evento"].ToString(),
                            estado_evento = dr["estado_evento"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_evento"]),
                            fecha_evento = dr["fecha_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fecha_evento"])
                            //accion_evento = DateTime.MinValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar eventos: " + ex.Message);
                lista = new List<Evento>();
            }
        }

        return lista;
    }

    public int Registrar(Evento obj, out string Mensaje)
    {
        int idEventoGenerado = 0;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_REGISTRAREVENTO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NombreEvento", obj.nombre_evento);
                    cmd.Parameters.AddWithValue("@FechaProgramadaEvento", obj.fechaprogramada_evento == DateTime.MinValue ? (object)DBNull.Value : obj.fechaprogramada_evento);
                    cmd.Parameters.AddWithValue("@FechaLimiteEvento", obj.fechalimite_evento == DateTime.MinValue ? (object)DBNull.Value : obj.fechalimite_evento);
                    cmd.Parameters.AddWithValue("@DescripcionEvento", (object)obj.descripcion_evento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstadoEvento", obj.estado_evento);

                    cmd.Parameters.Add("@IdEventoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idEventoGenerado = Convert.ToInt32(cmd.Parameters["@IdEventoResultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            idEventoGenerado = 0;
            Mensaje = ex.Message;
        }

        return idEventoGenerado;
    }

    public bool Editar(Evento obj, out string Mensaje)
    {
        bool resultado = false;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EDITAREVENTO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdEvento", obj.id_evento);
                    cmd.Parameters.AddWithValue("@NombreEvento", obj.nombre_evento);
                    cmd.Parameters.AddWithValue("@FechaProgramadaEvento", obj.fechaprogramada_evento == DateTime.MinValue ? (object)DBNull.Value : obj.fechaprogramada_evento);
                    cmd.Parameters.AddWithValue("@FechaLimiteEvento", obj.fechalimite_evento == DateTime.MinValue ? (object)DBNull.Value : obj.fechalimite_evento);
                    cmd.Parameters.AddWithValue("@DescripcionEvento", (object)obj.descripcion_evento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstadoEvento", obj.estado_evento);

                    cmd.Parameters.Add("@Respuesta", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Respuesta"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            resultado = false;
            Mensaje = ex.Message;
        }

        return resultado;
    }

    public bool Eliminar(int idEvento, out string Mensaje)
    {
        bool resultado = false;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINAREVENTO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEvento", idEvento);

                    cmd.Parameters.Add("@Respuesta", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Respuesta"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            resultado = false;
            Mensaje = ex.Message;
        }

        return resultado;
    }
}
