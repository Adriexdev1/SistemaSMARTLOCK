using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Evento
    {
        public int id_evento { get; set; }
        public string nombre_evento { get; set; }
        public DateTime fechaprogramada_evento { get; set; }
        public DateTime fechalimite_evento { get; set; }
        public string descripcion_evento { get; set; }
        public int estado_evento { get; set; }
        public DateTime fecha_evento { get; set; }

        public DateTime accion_evento { get; set; }
    }
}
