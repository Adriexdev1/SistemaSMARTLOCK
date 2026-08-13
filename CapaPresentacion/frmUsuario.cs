using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmUsuario : Form
    {

        public static string txtindice = "-1";

        public frmUsuario()
        {
            InitializeComponent();
        }


        private async void frmUsuario_Load(object sender, EventArgs e)
        {
         
            cboestado.Items.Add(new OpcionCmb() { valor = 1, texto = "Activo" });
            cboestado.Items.Add(new OpcionCmb() { valor = 0, texto = "No activo" });
            cboestado.DisplayMember = "texto";
            cboestado.ValueMember = "valor";
            cboestado.SelectedIndex = 0;

            List<Rol> lista_roles = await new CN_Rol().Listar();
            

            foreach (Rol item in lista_roles)
            {
                if (item.estado_rol != 0)
                {
                    cborol.Items.Add(new OpcionCmb() { valor = item.id_rol, texto = item.nombre_rol });

                }
            }

            cborol.DisplayMember = "texto";
            cborol.ValueMember = "valor";
            cborol.SelectedIndex = 0;


            ocultarCampos();

            foreach (DataGridViewColumn col in dgv1.Columns)
            {
                if (col.Visible && col.HeaderText != "")
                {
                    cmbfiltro.Items.Add(new OpcionCmb() { valor = col.Name, texto = col.HeaderText });
                }
            }
            cmbfiltro.DisplayMember = "texto";
            cmbfiltro.ValueMember = "valor";
            cmbfiltro.SelectedIndex = 0;

            List<Usuario> lista_usuarios = await new CN_Usuario().Listar();

            foreach (Usuario item in lista_usuarios)
            {
                dgv1.Rows.Add(new object[] { 
                    "", item.id_usuario, 
                    item.nombre_usuario, 
                    item.correo_usuario, 
                    item.telefono_usuario, 
                    item.rol.nombre_rol, 
                    item.contrasena_usuario, 
                    item.nacimiento_usuario.ToString(), 
                    item.estado_usuario == 1 ? "Activo" : "No activo", 
                    item.fecha_usuario.ToString(),
                    item.rol.id_rol,
                    item.estado_usuario
                
                });
            }

            cborol.DisplayMember = "texto";
            cborol.ValueMember = "valor";
            cborol.SelectedIndex = 0;

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Usuario user = new Usuario();
            user.id_usuario = string.IsNullOrEmpty(txtid.Text) ? 0 : Convert.ToInt32(txtid.Text);
            user.nombre_usuario = txtnombre.Text;
            user.correo_usuario = txtcorreo.Text;
            user.telefono_usuario = txttel.Text;
            user.rol = new Rol() {id_rol = (int)((OpcionCmb)cborol.SelectedItem).valor };
            user.estado_usuario = (int)((OpcionCmb)cboestado.SelectedItem).valor;
            user.contrasena_usuario = txtcontra.Text;
            user.nacimiento_usuario = Convert.ToDateTime(txtfecha.Text);

            if(user.id_usuario == 0)
            {
                int id_newuser = new CN_Usuario().Registrar(user, out mensaje);
                if (id_newuser != 0)
                {
                    dgv1.Rows.Add(new object[] { "", id_newuser, txtnombre.Text, txtcorreo.Text, txttel.Text, ((OpcionCmb)cborol.SelectedItem).texto, txtcontra.Text, txtfecha.Value.ToString(), ((OpcionCmb)cboestado.SelectedItem).texto, txtfecha.Value, ((OpcionCmb)cborol.SelectedItem).valor, ((OpcionCmb)cboestado.SelectedItem).valor });
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                

            } else
            {
                bool resultado = new CN_Usuario().Editar(user, out mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgv1.Rows[Convert.ToInt32(txtindice)];
                    row.Cells["idusuario"].Value = txtid.Text;
                    row.Cells["nombreusuario"].Value = txtnombre.Text;
                    row.Cells["correousuario"].Value = txtcorreo.Text;
                    row.Cells["telusuario"].Value = txttel.Text;
                    row.Cells["claveusuario"].Value = txtcontra.Text;
                    row.Cells["fechanacimientousuario"].Value = txtfecha.Value;
                    row.Cells["registrousuario"].Value = txtreg.Text;
                    row.Cells["rolusuario"].Value = ((OpcionCmb)cborol.SelectedItem).texto.ToString();
                    row.Cells["estadousuario"].Value = ((OpcionCmb)cboestado.SelectedItem).texto.ToString();


                } else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            Limpiar();

        }

        private async void ocultarCampos()
        {
            List<Rol> lista_roles = await new CN_Rol().Listar();
            int valor = Convert.ToInt32(((OpcionCmb)cborol.SelectedItem).valor);
            foreach (Rol item in lista_roles)
            {
                if (item.id_rol == valor)
                {
                    if (item.permiso_rol == 0)
                    {
                        txtcontra.ReadOnly = true;
                        txtconf.ReadOnly = true;
                    }
                    else
                    {
                        txtcontra.ReadOnly = false;
                        txtconf.ReadOnly = false;
                    }
                }
            }
        }

        private void Limpiar()
        {
            txtid.Text = "0";
            txtnombre.Text = "";
            txtcorreo.Text = "";
            txttel.Text = "";
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;
            txtcontra.Text = "";
            txtconf.Text = "";
            txtfecha.Text = "";
            txtreg.Text = "";
            txtindice = "-1";
            txtnombre.Select();
            ocultarCampos();


        }

        private void dgv1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = 20;
                var h = 20;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.ok, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv1.Columns[e.ColumnIndex].Name == "btnseleccion")
            {
                int indice = e.RowIndex;
                txtindice = indice.ToString();
                if (indice >= 0)
                {
                    txtid.Text = dgv1.Rows[indice].Cells["idusuario"].Value.ToString();
                    txtnombre.Text = dgv1.Rows[indice].Cells["nombreusuario"].Value.ToString(); ;
                    txtcorreo.Text = dgv1.Rows[indice].Cells["correousuario"].Value.ToString(); ;
                    txttel.Text = dgv1.Rows[indice].Cells["telusuario"].Value.ToString();
                    cborol.SelectedIndex = 0;
                    cboestado.SelectedIndex = 0;
                    txtcontra.Text = dgv1.Rows[indice].Cells["claveusuario"].Value.ToString();
                    txtconf.Text = dgv1.Rows[indice].Cells["claveusuario"].Value.ToString();
                    txtfecha.Text = dgv1.Rows[indice].Cells["fechanacimientousuario"].Value.ToString();
                    txtreg.Text = dgv1.Rows[indice].Cells["registrousuario"].Value.ToString();
                    foreach (OpcionCmb item in cborol.Items)
                    {
                        if(item.texto.ToString() == dgv1.Rows[indice].Cells["rolusuario"].Value.ToString())
                        {
                            int indcombo = cborol.Items.IndexOf(item);
                            cborol.SelectedIndex = indcombo;
                            break;
                        }
                    }
                    foreach (OpcionCmb item in cboestado.Items)
                    {
                        if (item.texto.ToString() == dgv1.Rows[indice].Cells["estadousuario"].Value.ToString())
                        {
                            int indcombo = cboestado.Items.IndexOf(item);
                            cboestado.SelectedIndex = indcombo;
                            break;
                        }
                    }

                    ocultarCampos();

                }
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Limpiar();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //Limpiar();
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar este usuario?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Usuario newuser = new Usuario() { id_usuario = Convert.ToInt32(txtid.Text) };
                    string mensaje = string.Empty;
                    bool resultado = new CN_Usuario().Eliminar(newuser, out mensaje);
                    if (resultado)
                    {
                        dgv1.Rows.RemoveAt(Convert.ToInt32(txtindice));


                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string colfiltro = ((OpcionCmb)cmbfiltro.SelectedItem).valor.ToString();
            int coincidencias = 0;
            if (dgv1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv1.Rows)
                {
                    if (row.Cells[colfiltro].Value.ToString().Trim().ToUpper().Contains(txtbuscar.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                        coincidencias++;
                    } else
                    {
                        row.Visible = false;
                    }
                }

                if (coincidencias == 0) MessageBox.Show("No se encontraron coincidencias.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show("No hay datos para mostrar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbuscar.Text = "";
            foreach(DataGridViewRow row in dgv1.Rows)
            {
                row.Visible = true;
            }
        }

        private void btneye1_Click(object sender, EventArgs e)
        {
            if (txtcontra.PasswordChar == '*')
            {
                txtcontra.PasswordChar = '\0';
                btneye1.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else
            {
                txtcontra.PasswordChar = '*';
                btneye1.IconChar = FontAwesome.Sharp.IconChar.Eye;
            }
        }

        private void btneye2_Click(object sender, EventArgs e)
        {
            if (txtconf.PasswordChar == '*')
            {
                txtconf.PasswordChar = '\0';
                btneye2.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else
            {
                txtconf.PasswordChar = '*';
                btneye2.IconChar = FontAwesome.Sharp.IconChar.Eye;
            }
        }

        private void cborol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ocultarCampos();
        }
    }
}
