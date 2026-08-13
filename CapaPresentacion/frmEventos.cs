using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmEventos : Form
    {
        public static string txtindice = "-1";
        public static List<Detalle_Evento> detallesTemporales = new List<Detalle_Evento>();

        public frmEventos()
        {
            InitializeComponent();
        }

        private async void frmEventos_Load(object sender, EventArgs e)
        {
            cboestado.Items.Add(new OpcionCmb() { valor = 1, texto = "Activo" });
            cboestado.Items.Add(new OpcionCmb() { valor = 0, texto = "Inactivo" });
            cboestado.DisplayMember = "texto";
            cboestado.ValueMember = "valor";
            cboestado.SelectedIndex = 0;
            cmbfiltro.Items.Clear();
            foreach (DataGridViewColumn col in dgv1.Columns)
            {
                if (col.Visible && col.HeaderText != "")
                    cmbfiltro.Items.Add(new OpcionCmb() { valor = col.Name, texto = col.HeaderText });
            }
            cmbfiltro.DisplayMember = "texto";
            cmbfiltro.ValueMember = "valor";
            cmbfiltro.SelectedIndex = 0;

            List<Evento> lista = await new CN_Evento().Listar();
            dgv1.Rows.Clear();
            foreach (Evento ev in lista)
            {
                dgv1.Rows.Add(
                    "",
                    ev.id_evento,
                    ev.nombre_evento,
                    ev.descripcion_evento,
                    ev.fechaprogramada_evento.ToString("g"),
                    ev.fechalimite_evento.ToString("g"),
                    ev.estado_evento == 1 ? "Activo" : "Inactivo",
                    ev.fecha_evento.ToString("g"),
                    ev.estado_evento
                    //ev.accion_evento != DateTime.MinValue ? ev.accion_evento.ToString("yyyy-MM-dd HH:mm:ss") : "Sin registro"


                );
            }
            await EsperarColumnasCargadasAsync();
            IniciarRefresco();
        }

        private CancellationTokenSource refrescarToken;

        private async void IniciarRefresco()
        {
            refrescarToken = new CancellationTokenSource();
            await RefrescarAsync(refrescarToken.Token);
        }

        private async Task RefrescarAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //DETENER SI NO HAY COLUMNAS AÚN
                if (!await EsperarColumnasCargadasAsync())
                {
                    await Task.Delay(500);
                    continue;
                }

                List<Evento> lista = await new CN_Evento().Listar();

                dgv1.Rows.Clear();

                foreach (Evento ev in lista)
                {
                    dgv1.Rows.Add(
                        "",
                        ev.id_evento,
                        ev.nombre_evento,
                        ev.descripcion_evento,
                        ev.fechaprogramada_evento.ToString("g"),
                        ev.fechalimite_evento.ToString("g"),
                        ev.estado_evento == 1 ? "Activo" : "Inactivo",
                        ev.fecha_evento.ToString("g"),
                        ev.estado_evento
                        //ev.accion_evento != DateTime.MinValue ? ev.accion_evento.ToString("dd/MM/yyyy HH:mm:ss") : "Sin registro"
                    );
                }

                await Task.Delay(10000, token);
            }
        }


        private async Task EsperarColumnasAsync()
        {
            while (dgv1.Columns.Count == 0)
            {
                await Task.Delay(50);
            }
        }

        private async Task<bool> EsperarColumnasCargadasAsync()
        {
            int intentos = 0;

            while (dgv1.Columns.Count == 0)
            {
                await Task.Delay(50);
                intentos++;

                // seguridad: si pasan 5 segundos y no hay columnas, aborta
                if (intentos > 100)
                    return false;
            }

            return true;
        }



        private void frmEventos_FormClosing(object sender, FormClosingEventArgs e)
        {
            refrescarToken?.Cancel();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Evento evento = new Evento()
            {
                id_evento = string.IsNullOrEmpty(txtid.Text) ? 0 : Convert.ToInt32(txtid.Text),
                nombre_evento = txtnombre.Text,
                descripcion_evento = txtdesc.Text,
                fechaprogramada_evento = txtfechainicio.Value.Date + txthorainicio.Value.TimeOfDay,
                fechalimite_evento = txtfechafin.Value.Date + txthorafin.Value.TimeOfDay,
                estado_evento = (int)((OpcionCmb)cboestado.SelectedItem).valor,
                fecha_evento = DateTime.Now
            };

            if (evento.id_evento == 0)
            {
                int id = new CN_Evento().Registrar(evento, out mensaje);
                if (id != 0)
                {
                    if (detallesTemporales != null && detallesTemporales.Count > 0)
                    {
                        CN_DetalleEvento cnDetalle = new CN_DetalleEvento();
                        foreach (var det in detallesTemporales)
                        {
                            det.evento.id_evento = id;
                            cnDetalle.Registrar(det, out string msgDetalle);
                        }

                        detallesTemporales.Clear();
                    }
                    dgv1.Rows.Add(new object[] {
                        "",
                        id,
                        evento.nombre_evento,
                        evento.descripcion_evento,
                        evento.fechaprogramada_evento.ToString("g"),
                        evento.fechalimite_evento.ToString("g"),
                        ((OpcionCmb)cboestado.SelectedItem).texto,
                        evento.fecha_evento.ToString("g"),
                        evento.estado_evento
                    }
                    );

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                bool resultado = new CN_Evento().Editar(evento, out mensaje);
                if (resultado)
                {

                    DataGridViewRow row = dgv1.Rows[Convert.ToInt32(txtindice)];
                    row.Cells["id"].Value = evento.id_evento;
                    row.Cells["nombre"].Value = evento.nombre_evento;
                    row.Cells["desc"].Value = evento.descripcion_evento;
                    row.Cells["inicioevento"].Value = evento.fechaprogramada_evento.ToString("g");
                    row.Cells["finevento"].Value = evento.fechalimite_evento.ToString("g");
                    row.Cells["estado"].Value = ((OpcionCmb)cboestado.SelectedItem).texto;
                    row.Cells["registro"].Value = evento.fecha_evento.ToString("g");
                    row.Cells["noestado"].Value = evento.estado_evento;

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void Limpiar()
        {
            txtid.Text = "0";
            txtnombre.Text = "";
            txtdesc.Text = "";
            txtfechainicio.Value = DateTime.Now;
            txthorainicio.Value = DateTime.Now;
            txtfechafin.Value = DateTime.Now;
            txthorafin.Value = DateTime.Now;
            cboestado.SelectedIndex = 0;
            txtreg.Text = "";
            txtindice = "-1";
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "0")
            {
                if (MessageBox.Show("¿Desea eliminar este evento?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Evento ev = new Evento() { id_evento = Convert.ToInt32(txtid.Text) };
                    string mensaje = string.Empty;
                    bool resultado = new CN_Evento().Eliminar(ev, out mensaje);
                    if (resultado)
                    {
                        dgv1.Rows.RemoveAt(Convert.ToInt32(txtindice));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string colfiltro = ((OpcionCmb)cmbfiltro.SelectedItem).valor.ToString();
            int coincidencias = 0;
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                if (row.Cells[colfiltro].Value.ToString().ToUpper().Contains(txtbuscar.Text.ToUpper()))
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

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbuscar.Text = "";
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                row.Visible = true;
            }
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

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv1.Columns[e.ColumnIndex].Name == "btnseleccion" && e.RowIndex >= 0)
            {
                txtindice = e.RowIndex.ToString();
                DataGridViewRow row = dgv1.Rows[e.RowIndex];
                txtid.Text = row.Cells["id"].Value.ToString();
                txtnombre.Text = row.Cells["nombre"].Value.ToString();
                txtdesc.Text = row.Cells["desc"].Value.ToString();
                DateTime inicio = Convert.ToDateTime(row.Cells["inicioevento"].Value);
                txtfechainicio.Value = inicio.Date;
                txthorainicio.Value = inicio;
                DateTime fin = Convert.ToDateTime(row.Cells["finevento"].Value);
                txtfechafin.Value = fin.Date;
                txthorafin.Value = fin;
                txtreg.Text = row.Cells["registro"].Value.ToString();
                foreach (OpcionCmb item in cboestado.Items)
                {
                    if (item.texto == row.Cells["estado"].Value.ToString())
                    {
                        cboestado.SelectedItem = item;
                        break;
                    }
                }
            }
        }


        private void btnusuarios_Click(object sender, EventArgs e)
        {
            int idEvento = string.IsNullOrEmpty(txtid.Text) ? 0 : Convert.ToInt32(txtid.Text);
            frmUsuariosEvento ventana = new frmUsuariosEvento(idEvento);
            ventana.ShowDialog();
            if (idEvento == 0 && ventana.DetallesPendientes != null)
            {
                detallesTemporales.Clear();
                detallesTemporales.AddRange(ventana.DetallesPendientes);
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
