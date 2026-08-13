using CapaEntidad;
using FontAwesome.Sharp;
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
    public partial class Inicio : Form
    {
        public static Usuario usuario_actual = new Usuario();
        private static IconButton menu_actual = null;
        private static Form formulario_actual = null;

        public Inicio(Usuario usuario_login = null)
        {
            if(usuario_login == null)
            {
                usuario_login = new Usuario()
                {
                    nombre_usuario = "Admin (DEFAULT)",
                    correo_usuario = "a",
                    id_usuario = 1
                };
            }
            InitializeComponent();
            usuario_actual = usuario_login;
            nomUsuario.Text = $"USUARIO: {usuario_actual.nombre_usuario}";
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AbrirMenu(IconButton menu, Form formulario)
        {
            if (menu_actual != null)
            {
                menu_actual.BackColor = Color.FromArgb(0,0,64);
            }
            menu.BackColor = Color.Blue;
            menu_actual = menu;

            if(formulario_actual != null)
            {
                formulario_actual.Close();
            }

            formulario_actual = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            //formulario.BackColor = Color.SteelBlue;
            contenedor.Controls.Add(formulario);
            formulario.Show();

        }

        private void menu_usuarios_Click(object sender, EventArgs e)
        {
            AbrirMenu((IconButton)sender, new frmUsuario());
        }

        private void menu_eventos_Click(object sender, EventArgs e)
        {
            AbrirMenu((IconButton)sender, new frmEventos());
        }

        private void menu_roles_Click(object sender, EventArgs e)
        {
            AbrirMenu((IconButton)sender, new frmRoles());
        }

        private void menu_ajustes_Click(object sender, EventArgs e)
        {
            AbrirMenu((IconButton)sender, new frmAjustes());
        }

        private void menu_acercade_Click(object sender, EventArgs e)
        {
            AbrirMenu((IconButton)sender, new frmAcercade());
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            if(usuario_actual.rol.permiso_rol < 2)
            {
                menu_roles.Visible = false;
            }
        }
    }
}
