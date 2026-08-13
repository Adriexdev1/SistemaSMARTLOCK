using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public DateTime nacimiento_usuario { get; set; }
        public string telefono_usuario  { get; set; }
        public string correo_usuario { get; set; }
        public Rol rol { get; set; }
        public int estado_usuario { get; set; }
        public DateTime fecha_usuario { get; set; }
        public string contrasena_usuario { get; set; }
        

    }
}
