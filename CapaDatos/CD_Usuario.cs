using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CD_Usuario
{
    public async Task<List<Usuario>> Listar()
    {
        List<Usuario> lista = new List<Usuario>();
        using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
        {
            try
            {
                string query = "SELECT u.id_usuario, u.nombre_usuario, u.telefono_usuario, u.correo_usuario, u.contrasena_usuario, u.estado_usuario, u.fecha_usuario, u.nacimiento_usuario, r.id_rol, r.nombre_rol, r.descripcion_rol, r.fecha_rol, r.estado_rol, r.permiso_rol FROM usuarios u inner join roles r on r.id_rol = u.id_rol";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                await oconexion.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Usuario()
                        {
                            id_usuario = Convert.ToInt32(dr["id_usuario"]),
                            nombre_usuario = dr["nombre_usuario"].ToString(),
                            correo_usuario = dr["correo_usuario"].ToString(),
                            contrasena_usuario = dr["contrasena_usuario"] == DBNull.Value ? "" : dr["contrasena_usuario"].ToString(),
                            telefono_usuario = dr["telefono_usuario"] == DBNull.Value ? "" : dr["telefono_usuario"].ToString(),
                            estado_usuario = dr["estado_usuario"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_usuario"]),
                            nacimiento_usuario = dr["nacimiento_usuario"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["nacimiento_usuario"]),
                            fecha_usuario = dr["fecha_usuario"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dr["fecha_usuario"]),
                            rol = new Rol()
                            {
                                id_rol = Convert.ToInt32(dr["id_rol"]),
                                nombre_rol = dr["nombre_rol"].ToString(),
                                descripcion_rol = dr["descripcion_rol"] == DBNull.Value ? "" : dr["descripcion_rol"].ToString(),
                                estado_rol = dr["estado_rol"] == DBNull.Value ? 0 : Convert.ToInt32(dr["estado_rol"]),
                                fecha_rol = Convert.ToDateTime(dr["fecha_rol"]),
                                permiso_rol = dr["permiso_rol"] == DBNull.Value ? 0 : Convert.ToInt32(dr["permiso_rol"])
                            }
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar usuarios: " + ex.Message);
                lista = new List<Usuario>();
            }
        }
        return lista;
    }

    public int Registrar(Usuario obj, out string Mensaje)
    {
        int idUsuarioGenerado = 0;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreUsuario", obj.nombre_usuario);
                    cmd.Parameters.AddWithValue("@TelefonoUsuario", (object)obj.telefono_usuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CorreoUsuario", obj.correo_usuario);
                    cmd.Parameters.AddWithValue("@ContrasenaUsuario", (object)obj.contrasena_usuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", obj.rol.id_rol);
                    cmd.Parameters.AddWithValue("@EstadoUsuario", obj.estado_usuario);
                    cmd.Parameters.AddWithValue("@NacimientoUsuario", obj.nacimiento_usuario == DateTime.MinValue ? (object)DBNull.Value : obj.nacimiento_usuario);

                    cmd.Parameters.Add("@IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idUsuarioGenerado = Convert.ToInt32(cmd.Parameters["@IdUsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            idUsuarioGenerado = 0;
            Mensaje = ex.Message;
        }

        return idUsuarioGenerado;
    }

    public bool Editar(Usuario obj, out string Mensaje)
    {
        bool resultado = false;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", obj.id_usuario);
                    cmd.Parameters.AddWithValue("@NombreUsuario", obj.nombre_usuario);
                    cmd.Parameters.AddWithValue("@TelefonoUsuario", (object)obj.telefono_usuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CorreoUsuario", obj.correo_usuario);
                    cmd.Parameters.AddWithValue("@ContrasenaUsuario", (object)obj.contrasena_usuario ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", obj.rol.id_rol);
                    cmd.Parameters.AddWithValue("@EstadoUsuario", obj.estado_usuario);
                    cmd.Parameters.AddWithValue("@NacimientoUsuario", obj.nacimiento_usuario == DateTime.MinValue ? (object)DBNull.Value : obj.nacimiento_usuario);

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

    public bool Eliminar(int idUsuario, out string Mensaje)
    {
        bool resultado = false;
        Mensaje = string.Empty;

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ELIMINARUSUARIO", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

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