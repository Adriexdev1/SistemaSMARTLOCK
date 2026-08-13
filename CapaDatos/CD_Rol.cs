using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Rol
    {
        public async Task<List<Rol>> Listar()
        {
            List<Rol> lista = new List<Rol>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "SELECT id_rol, nombre_rol, descripcion_rol, estado_rol, fecha_rol, permiso_rol FROM roles";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    await oconexion.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            lista.Add(new Rol()
                            {
                                id_rol = Convert.ToInt32(dr["id_rol"]),
                                nombre_rol = dr["nombre_rol"].ToString(),
                                descripcion_rol = dr["descripcion_rol"].ToString(),
                                estado_rol = Convert.ToInt32(dr["estado_rol"]),
                                fecha_rol = Convert.ToDateTime(dr["fecha_rol"]),
                                permiso_rol = Convert.ToInt32(dr["permiso_rol"]),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Rol>();
                }
            }
            return lista;
        }

        public int Registrar(Rol obj, out string Mensaje)
        {
            int idRolGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REGISTRARROL", oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombreRol", obj.nombre_rol);
                        cmd.Parameters.AddWithValue("@DescripcionRol", (object)obj.descripcion_rol ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EstadoRol", obj.estado_rol);
                        cmd.Parameters.AddWithValue("@PermisoRol", obj.permiso_rol);

                        cmd.Parameters.Add("@IdRolResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        oconexion.Open();
                        cmd.ExecuteNonQuery();

                        idRolGenerado = Convert.ToInt32(cmd.Parameters["@IdRolResultado"].Value);
                        Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                idRolGenerado = 0;
                Mensaje = ex.Message;
            }

            return idRolGenerado;
        }

        public bool Editar(Rol obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EDITARROL", oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdRol", obj.id_rol);
                        cmd.Parameters.AddWithValue("@NombreRol", obj.nombre_rol);
                        cmd.Parameters.AddWithValue("@DescripcionRol", (object)obj.descripcion_rol ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EstadoRol", obj.estado_rol);
                        cmd.Parameters.AddWithValue("@PermisoRol", obj.permiso_rol);

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

        public bool Eliminar(int idRol, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ELIMINARROL", oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdRol", idRol);

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
}
