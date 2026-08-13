using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmRoles : Form
    {
        private CN_Rol oCN_Rol = new CN_Rol();
        private Rol rolSeleccionado = null;

        public frmRoles()
        {
            InitializeComponent();
        }

        private void frmRoles_Load(object sender, EventArgs e)
        {
            CargarRoles();
            txtid.Text = "0";
            cbopermiso.Items.Add(0);
            cbopermiso.Items.Add(1);
            cbopermiso.Items.Add(2);
            cbopermiso.SelectedIndex = 0;

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


            cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;
        }

        private async void CargarRoles()
        {
            dgv1.Rows.Clear();
            List<Rol> listaRoles = await oCN_Rol.Listar();

            foreach (Rol item in listaRoles)
            {
                dgv1.Rows.Add("",
                    item.id_rol,
                    item.nombre_rol,
                    item.descripcion_rol,
                    item.permiso_rol,
                    item.estado_rol == 1 ? "Activo" : "Inactivo",
                    item.fecha_rol.ToString("dd/MM/yyyy"),
                    item.estado_rol);
            }
        }

        private void LimpiarCampos()
        {
            rolSeleccionado = null;
            txtid.Text = "0";
            txtnombre.Text = "";
            txtdescripcion.Text = "";
            cbopermiso.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;
            txtreg.Text = "";
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Rol rol = new Rol();
            rol.id_rol = Convert.ToInt32(txtid.Text);
            rol.nombre_rol = txtnombre.Text.Trim();
            rol.descripcion_rol = txtdescripcion.Text.Trim();
            rol.permiso_rol = Convert.ToInt32(cbopermiso.SelectedItem);
            rol.estado_rol = (int)((OpcionCombo)cboestado.SelectedItem).Valor;
            rol.fecha_rol = DateTime.Now;

            if (rol.id_rol == 0)
            {
                // Registrar nuevo rol
                int id_newrol = new CN_Rol().Registrar(rol, out mensaje);
                if (id_newrol != 0)
                {
                    dgv1.Rows.Add(new object[]
                    {
                "",
                id_newrol,
                txtnombre.Text,
                txtdescripcion.Text,
                cbopermiso.SelectedItem.ToString(),
                ((OpcionCombo)cboestado.SelectedItem).Texto,
                rol.fecha_rol.ToString("dd/MM/yyyy"),
                ((OpcionCombo)cboestado.SelectedItem).Valor
                    });

                    MessageBox.Show("Rol registrado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                LimpiarCampos();
            }
            else
            {
                // Editar rol existente
                bool resultado = new CN_Rol().Editar(rol, out mensaje);
                if (resultado)
                {
                    // Buscar la fila correspondiente al índice del DataGridView
                    foreach (DataGridViewRow row in dgv1.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["idrol"].Value) == rol.id_rol)
                        {
                            row.Cells["nombrerol"].Value = txtnombre.Text;
                            row.Cells["descripcionrol"].Value = txtdescripcion.Text;
                            row.Cells["permisorol"].Value = cbopermiso.SelectedItem.ToString();
                            row.Cells["estadorol"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto;
                            row.Cells["registrorol"].Value = rol.fecha_rol.ToString("dd/MM/yyyy");
                            row.Cells["noestadorol"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor;
                            break;
                        }
                    }

                    MessageBox.Show("Rol editado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }



        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (rolSeleccionado != null)
            {
                if (MessageBox.Show("¿Desea eliminar el rol seleccionado?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje;
                    bool resultado = oCN_Rol.Eliminar(rolSeleccionado, out mensaje);

                    if (resultado)
                    {
                        dgv1.Rows.RemoveAt(dgv1.SelectedRows[0].Index);
                        MessageBox.Show("Rol eliminado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un rol para eliminar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgv1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

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

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv1.Columns[e.ColumnIndex].Name == "btnseleccion")
            {
                int indice = e.RowIndex;

                rolSeleccionado = new Rol()
                {
                    id_rol = Convert.ToInt32(dgv1.Rows[indice].Cells["idrol"].Value),
                    nombre_rol = dgv1.Rows[indice].Cells["nombrerol"].Value.ToString(),
                    descripcion_rol = dgv1.Rows[indice].Cells["descripcionrol"].Value.ToString(),
                    permiso_rol = Convert.ToInt32(dgv1.Rows[indice].Cells["permisorol"].Value),
                    estado_rol = Convert.ToInt32(dgv1.Rows[indice].Cells["noestadorol"].Value),
                    fecha_rol = DateTime.Parse(dgv1.Rows[indice].Cells["registrorol"].Value.ToString())
                };

                txtid.Text = rolSeleccionado.id_rol.ToString();
                txtnombre.Text = rolSeleccionado.nombre_rol;
                txtdescripcion.Text = rolSeleccionado.descripcion_rol;
                cbopermiso.SelectedItem = rolSeleccionado.permiso_rol;
                cboestado.SelectedIndex = rolSeleccionado.estado_rol == 1 ? 0 : 1;
                txtreg.Text = rolSeleccionado.fecha_rol.ToString("dd/MM/yyyy");
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
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }

                if (coincidencias == 0) MessageBox.Show("No se encontraron coincidencias.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay datos para mostrar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbuscar.Text = "";
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                row.Visible = true;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }

    // Clase auxiliar para combo de estado
    public class OpcionCombo
    {
        public int Valor { get; set; }
        public string Texto { get; set; }
        public override string ToString() => Texto;
    }
}
