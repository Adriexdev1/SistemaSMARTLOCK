using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Evento
    {
        public int id_detalle_evento {  get; set; }
        public Usuario usuario { get; set; }
        public Evento evento { get; set; }
        public DateTime fecha_detalle_evento { get; set; }
        public string rol_en_evento { get; set; }
        public DateTime? acceso_detalle_evento { get; set; }
    }
}
