using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Evento
    {
        private CD_Evento ocd_evento = new CD_Evento();

        public async Task<List<Evento>> Listar()
        {
            return await ocd_evento.Listar();
        }

        public int Registrar(Evento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre_evento))
                Mensaje += "Es necesario el nombre del evento.\n";

            if (obj.estado_evento < 0)
                Mensaje += "Debe seleccionar un estado válido para el evento.\n";

            if (Mensaje != string.Empty)
                return 0;

            return ocd_evento.Registrar(obj, out Mensaje);
        }

        public bool Editar(Evento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.id_evento <= 0)
                Mensaje += "Evento no válido.\n";

            if (string.IsNullOrEmpty(obj.nombre_evento))
                Mensaje += "Es necesario el nombre del evento.\n";

            if (obj.estado_evento < 0)
                Mensaje += "Debe seleccionar un estado válido para el evento.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_evento.Editar(obj, out Mensaje);
        }

        public bool Eliminar(Evento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.id_evento < 0)
                Mensaje += "Evento no válido.\n";

            if (Mensaje != string.Empty)
                return false;

            return ocd_evento.Eliminar(obj.id_evento, out Mensaje);
        }
    }
}
