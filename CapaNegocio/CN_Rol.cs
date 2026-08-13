using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Rol
    {
        private CD_Rol ocd_rol = new CD_Rol();

        public async Task<List<Rol>> Listar()
        {
            return await ocd_rol.Listar();
        }

        public int Registrar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre_rol))
                Mensaje += "Es necesario el nombre del rol.\n";

            if (Mensaje != string.Empty)
                return 0;

            return ocd_rol.Registrar(obj, out Mensaje);
        }

        public bool Editar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre_rol))
                Mensaje += "Es necesario el nombre del rol.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_rol.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.id_rol <= 0)
                Mensaje += "Rol no válido.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_rol.Eliminar(obj.id_rol, out Mensaje);
        }

    }
}
