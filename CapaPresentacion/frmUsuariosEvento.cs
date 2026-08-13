using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmUsuariosEvento : Form
    {
        public List<Detalle_Evento> DetallesPendientes { get; set; } = new List<Detalle_Evento>();
        public int idEventoActual = 0;

        private CancellationTokenSource refrescarToken;

        public frmUsuariosEvento(int idEvento)
        {
            InitializeComponent();
            idEventoActual = idEvento;
            DetallesPendientes = new List<Detalle_Evento>(frmEventos.detallesTemporales);
        }

        private async void frmUsuariosEvento_Load(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
            await CargarAsignadosAsync();

            foreach (DataGridViewColumn col in dgv1.Columns)
            {
                if (col.Visible && col.HeaderText != "")
                    cmbfiltro.Items.Add(new OpcionCmb() { valor = col.Name, texto = col.HeaderText });
            }
            cmbfiltro.DisplayMember = "texto";
            cmbfiltro.ValueMember = "valor";
            cmbfiltro.SelectedIndex = 0;
            await EsperarColumnasCargadasAsync();
            IniciarRefresco();
        }

        private async Task CargarUsuariosAsync()
        {
            dgv1.Rows.Clear();
            List<Usuario> lista_usuarios = await new CN_Usuario().Listar();

            foreach (Usuario item in lista_usuarios)
            {
                if (item.estado_usuario != 0)
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
            }
        }

        private async Task CargarAsignadosAsync()
        {
            dgv2.Rows.Clear();

            if (idEventoActual == 0)
            {
                dgv2.Columns["qr"].Visible = false;
                dgv2.Columns["iddetalle"].Visible = false;
                if (DetallesPendientes != null && DetallesPendientes.Count > 0)
                {
                    foreach (var det in DetallesPendientes)
                    {
                        dgv2.Rows.Add("", det.usuario.id_usuario, det.usuario.nombre_usuario, 0, "Sin registro", "");
                    }
                }
            }
            else
            {
                List<Detalle_Evento> lista = await new CN_DetalleEvento().Listar(idEventoActual);

                foreach (var det in lista)
                {
                    string acceso = det.acceso_detalle_evento?.ToString("yyyy-MM-dd HH:mm") ?? "Sin registro";

                    dgv2.Rows.Add("", det.usuario.id_usuario, det.usuario.nombre_usuario, det.id_detalle_evento, det.acceso_detalle_evento?.ToString("yyyy-MM-dd HH:mm") ?? "Sin registro", "");
                }
            }
        }

        private void IniciarRefresco()
        {
            refrescarToken = new CancellationTokenSource();
            _ = RefrescarAsync(refrescarToken.Token);
        }

        private async Task RefrescarAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!await EsperarColumnasCargadasAsync())
                {
                    await Task.Delay(500, token);
                    continue;
                }

                try
                {
                    await CargarUsuariosAsync();
                    await CargarAsignadosAsync();
                }
                catch
                {

                }

                await Task.Delay(10000, token);
            }
        }

        private async Task<bool> EsperarColumnasCargadasAsync()
        {
            int intentos = 0;
            while (dgv1.Columns.Count == 0 || dgv2.Columns.Count == 0)
            {
                await Task.Delay(50);
                intentos++;
                if (intentos > 100) return false;
            }
            return true;
        }

        private void frmUsuariosEvento_FormClosing(object sender, FormClosingEventArgs e)
        {
            refrescarToken?.Cancel();
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

                if (coincidencias == 0)
                    MessageBox.Show("No se encontraron coincidencias.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hay datos para mostrar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgv1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = 20; var h = 20;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.ok, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgv2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = 20; var h = 20;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.trash, new Rectangle(x, y, w, h));
                e.Handled = true;
            }

            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = 20; var h = 20;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.eye, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgv1.Columns[e.ColumnIndex].Name == "btnseleccion")
            {
                int idUsuario = Convert.ToInt32(dgv1.Rows[e.RowIndex].Cells["idusuario"].Value);
                string nombreUsuario = dgv1.Rows[e.RowIndex].Cells["nombreusuario"].Value.ToString();
                string rolEnEvento = dgv1.Rows[e.RowIndex].Cells["rolusuario"].Value.ToString();

                if (dgv2.Rows.Cast<DataGridViewRow>().Any(r => Convert.ToInt32(r.Cells["id"].Value) == idUsuario))
                {
                    MessageBox.Show("Este usuario ya fue agregado al evento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Detalle_Evento detalle = new Detalle_Evento()
                {
                    id_detalle_evento = idEventoActual,
                    usuario = new Usuario { id_usuario = idUsuario, nombre_usuario = nombreUsuario },
                    evento = new Evento { id_evento = idEventoActual },
                    rol_en_evento = rolEnEvento,
                };

                if (idEventoActual > 0)
                {
                    string mensaje;
                    int idDetalle = new CN_DetalleEvento().Registrar(detalle, out mensaje);
                    if (idDetalle > 0)
                    {
                        dgv2.Rows.Add("", idUsuario, nombreUsuario, idDetalle, "Sin registro", "");
                        MessageBox.Show("Usuario agregado correctamente al evento.", "Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error al agregar usuario al evento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    DetallesPendientes.Add(detalle);
                    dgv2.Rows.Add("", detalle.usuario.id_usuario, detalle.usuario.nombre_usuario, 0, "Sin registro", "");
                    MessageBox.Show("Usuario agregado correctamente. Guarda el evento para confirmar los cambios.", "Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private async void dgv2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgv2.Columns[e.ColumnIndex].Name == "qr")
            {
                int idDetalle = Convert.ToInt32(dgv2.Rows[e.RowIndex].Cells["iddetalle"].Value);
                frmQR ventanaQR = new frmQR(idDetalle);
                ventanaQR.ShowDialog();
            }

            if (dgv2.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                int idUsuario = Convert.ToInt32(dgv2.Rows[e.RowIndex].Cells["id"].Value);

                if (MessageBox.Show("¿Deseas eliminar este usuario del evento?",
                                    "Confirmar eliminación",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (idEventoActual > 0)
                    {
                        try
                        {
                            var listaDetalles = await new CN_DetalleEvento().Listar(idEventoActual);

                            var detalleAEliminar = listaDetalles.FirstOrDefault(d => d.usuario.id_usuario == idUsuario);

                            if (detalleAEliminar != null)
                            {
                                string mensaje;
                                bool respuesta = new CN_DetalleEvento().Eliminar(detalleAEliminar, out mensaje);

                                if (respuesta)
                                {
                                    dgv2.Rows.RemoveAt(e.RowIndex);
                                    MessageBox.Show("Usuario eliminado correctamente del evento.", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo eliminar: " + mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el detalle a eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        var detalleTemp = DetallesPendientes.FirstOrDefault(d => d.usuario.id_usuario == idUsuario);
                        if (detalleTemp != null)
                        {
                            DetallesPendientes.Remove(detalleTemp);
                        }

                        dgv2.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbuscar.Text = "";
            foreach (DataGridViewRow row in dgv1.Rows)
                row.Visible = true;
        }
    }
}
