using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_DetalleEvento
    {
        private CD_DetalleEvento ocd_detalle = new CD_DetalleEvento();

        public async Task<List<Detalle_Evento>> Listar(int id)
        {
            return await ocd_detalle.Listar(id);
        }

        public int Registrar(Detalle_Evento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.usuario == null || obj.usuario.id_usuario <= 0)
                Mensaje += "Debe seleccionar un usuario válido.\n";

            if (obj.evento == null || obj.evento.id_evento <= 0)
                Mensaje += "Debe seleccionar un evento válido.\n";

            if (string.IsNullOrEmpty(obj.rol_en_evento))
                Mensaje += "Debe especificar un rol en el evento.\n";

            if (Mensaje != string.Empty)
                return 0;

            return ocd_detalle.Registrar(obj, out Mensaje);
        }


        public bool Eliminar(Detalle_Evento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.id_detalle_evento <= 0)
                Mensaje += "Detalle de evento no válido.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_detalle.Eliminar(obj.id_detalle_evento, out Mensaje);
        }
    }
}
