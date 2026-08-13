using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmAjustes : Form
    {
        Usuario usuario = Inicio.usuario_actual;
        public frmAjustes()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void frmAjustes_Load(object sender, EventArgs e)
        {
            nombre.Text = usuario.nombre_usuario;
            correo.Text = usuario.correo_usuario;
            telefono.Text = usuario.telefono_usuario;
            rol.Text = usuario.rol.nombre_rol;
            fechanacimiento.Text = usuario.nacimiento_usuario.ToString();
            fecharegistro.Text = usuario.fecha_usuario.ToString();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea salir del programa?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btncambiar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar esta ventana?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form formularioPadre = this.ParentForm;

                if (formularioPadre != null)
                {
                    formularioPadre.Close();
                }
                Login formulario = new Login();
                formulario.Show();
            }
        }
    }
}
