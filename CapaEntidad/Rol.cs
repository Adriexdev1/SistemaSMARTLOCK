using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Rol
    {
        public int id_rol {  get; set; }
        public string nombre_rol { get; set; }
        public string descripcion_rol { get; set; }
        public DateTime fecha_rol { get; set; }
        public int estado_rol { get; set; }
        public int permiso_rol { get; set; }

    }
}
