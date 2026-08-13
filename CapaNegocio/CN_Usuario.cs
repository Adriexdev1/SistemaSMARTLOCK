using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario ocd_usuario = new CD_Usuario();

        public async Task<List<Usuario>> Listar()
        {
            return await ocd_usuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.correo_usuario))
                Mensaje += "Es necesario el correo del usuario.\n";

            if (string.IsNullOrEmpty(obj.nombre_usuario))
                Mensaje += "Es necesario el nombre del usuario.\n";

            if (Mensaje != string.Empty)
                return 0;

            return ocd_usuario.Registrar(obj, out Mensaje);
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.correo_usuario))
                Mensaje += "Es necesario el correo del usuario.\n";

            if (string.IsNullOrEmpty(obj.nombre_usuario))
                Mensaje += "Es necesario el nombre del usuario.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_usuario.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.id_usuario <= 0)
                Mensaje += "Usuario no válido.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_usuario.Eliminar(obj.id_usuario, out Mensaje);
        }

    }
}
