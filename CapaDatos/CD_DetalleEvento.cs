using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
public class CD_DetalleEvento
{
    public async Task<List<Detalle_Evento>> Listar(int idEvento)
    {
        List<Detalle_Evento> lista = new List<Detalle_Evento>();

        using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
        {
            try
            {
                string query = @"
                SELECT 
                    de.id_detalle_evento,
                    de.id_usuario,
                    de.id_evento,
                    de.fecha_detalle_evento,
                    de.rol_en_evento,
                    de.acceso_detalle_evento,

                    u.nombre_usuario,
                    u.telefono_usuario,
                    u.correo_usuario,
                    u.id_rol,
                    u.estado_usuario,
                    u.fecha_usuario,
                    u.contrasena_usuario,
                    u.nacimiento_usuario,

                    e.nombre_evento,
                    e.fechaprogramada_evento,
                    e.fechalimite_evento,
                    e.descripcion_evento,
                    e.estado_evento,
                    e.fecha_evento
                FROM detalle_eventos de
                INNER JOIN usuarios u ON u.id_usuario = de.id_usuario
                INNER JOIN eventos e ON e.id_evento = de.id_evento
                WHERE de.id_evento = @idEvento";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.Parameters.AddWithValue("@idEvento", idEvento);

                await oconexion.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        var detalle = new Detalle_Evento
                        {
                            id_detalle_evento = Convert.ToInt32(dr["id_detalle_evento"]),
                            fecha_detalle_evento = dr["fecha_detalle_evento"] == DBNull.Value
                                ? DateTime.MinValue
                                : Convert.ToDateTime(dr["fecha_detalle_evento"]),
                            rol_en_evento = dr["rol_en_evento"] == DBNull.Value
                                ? string.Empty
                                : dr["rol_en_evento"].ToString(),
                            acceso_detalle_evento = dr["acceso_detalle_evento"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(dr["acceso_detalle_evento"]),
                            usuario = new Usuario
                            {
                                id_usuario = Convert.ToInt32(dr["id_usuario"]),
                                nombre_usuario = dr["nombre_usuario"] == DBNull.Value ? "" : dr["nombre_usuario"].ToString(),
                                telefono_usuario = dr["telefono_usuario"] == DBNull.Value ? "" : dr["telefono_usuario"].ToString(),
                                correo_usuario = dr["correo_usuario"] == DBNull.Value ? "" : dr["correo_usuario"].ToString(),
                                rol = new Rol
                                {
                                    id_rol = dr["id_rol"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id_rol"])
                                },
                                estado_usuario = dr["estado_usuario"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_usuario"]),
                                fecha_usuario = dr["fecha_usuario"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fecha_usuario"]),
                                contrasena_usuario = dr["contrasena_usuario"] == DBNull.Value ? "" : dr["contrasena_usuario"].ToString(),
                                nacimiento_usuario = dr["nacimiento_usuario"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["nacimiento_usuario"])
                            },

                            evento = new Evento
                            {
                                id_evento = Convert.ToInt32(dr["id_evento"]),
                                nombre_evento = dr["nombre_evento"] == DBNull.Value ? "" : dr["nombre_evento"].ToString(),
                                fechaprogramada_evento = dr["fechaprogramada_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechaprogramada_evento"]),
                                fechalimite_evento = dr["fechalimite_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fechalimite_evento"]),
                                descripcion_evento = dr["descripcion_evento"] == DBNull.Value ? "" : dr["descripcion_evento"].ToString(),
                                estado_evento = dr["estado_evento"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_evento"]),
                                fecha_evento = dr["fecha_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fecha_evento"])
                                //accion_evento = dr["accion_evento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["accion_evento"])
                            }
                        };

                        lista.Add(detalle);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar detalle de eventos: " + ex.Message);
                lista = new List<Detalle_Evento>();
            }
        }

        return lista;
    }




    public int Registrar(Detalle_Evento obj, out string Mensaje)
    {
        int idGenerado = 0;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_REGISTRAR_DETALLE_EVENTO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", obj.usuario.id_usuario);
                    cmd.Parameters.AddWithValue("@IdEvento", obj.evento.id_evento);
                    cmd.Parameters.AddWithValue("@Rol", obj.rol_en_evento ?? (object)DBNull.Value);
                    cmd.Parameters.Add("@IdResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idGenerado = Convert.ToInt32(cmd.Parameters["@IdResultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            idGenerado = 0;
            Mensaje = ex.Message;
        }

        return idGenerado;
    }

    

    public bool Eliminar(int idDetalleEvento, out string Mensaje)
    {
        bool resultado = false;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINAR_DETALLE_EVENTO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_detalle_evento", idDetalleEvento);

                    cmd.Parameters.Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@resultado"].Value);
                    Mensaje = cmd.Parameters["@mensaje"].Value.ToString();
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
