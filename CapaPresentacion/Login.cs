using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            txtid.Focus();
        }

        private async void btningresar_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtid.Text) || string.IsNullOrWhiteSpace(txtcontrasena.Text))
            {
                MessageBox.Show("Por favor, ingrese datos en los campos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<Usuario> usuarios = await new CN_Usuario().Listar();

            Usuario ousuario = new Usuario();

            if(int.TryParse(txtid.Text, out int id_buscado)) { 
                ousuario = usuarios.Where(u => u.id_usuario == Convert.ToInt32(id_buscado) && u.contrasena_usuario == txtcontrasena.Text && u.estado_usuario == 1).FirstOrDefault();
            }
            else
            {
                ousuario = usuarios.Where(u => u.correo_usuario == txtid.Text && u.contrasena_usuario == txtcontrasena.Text && u.estado_usuario == 1).FirstOrDefault();
            }

            if (ousuario != null)
            {
                if(ousuario.contrasena_usuario != "" || ousuario.contrasena_usuario != null)
                {
                    MessageBox.Show($"¡Bienvenido, {ousuario.nombre_usuario}!", "Ingreso exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Inicio form = new Inicio(ousuario);
                    this.Hide();
                    form.Show();
                    form.FormClosing += closingForm;
                }
                
            }
            else
            {
                string idsDisponibles = string.Join(", ", usuarios.Select(u => u.id_usuario));
                MessageBox.Show("Datos no válidos. Ingrese nuevamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closingForm(object sender, FormClosingEventArgs e)
        {
            txtid.Text = "";
            txtcontrasena.Text = "";
            this.Show();
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtid.Focus();
            txtid.Select();
        }

        private void btneye1_Click(object sender, EventArgs e)
        {
            if (txtcontrasena.PasswordChar == '*')
            {
                txtcontrasena.PasswordChar = '\0';
                btneye1.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else
            {
                txtcontrasena.PasswordChar = '*';
                btneye1.IconChar = FontAwesome.Sharp.IconChar.Eye;
            }
        }
    }
}
