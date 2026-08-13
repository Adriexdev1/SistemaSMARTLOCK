using QRCoder;
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
    public partial class frmQR : Form
    {
        private Bitmap qrImage;
        private int idDetalle;

        public frmQR(int idDetalle)
        {
            InitializeComponent();
            this.idDetalle = idDetalle;
        }

        private void frmQR_Load(object sender, EventArgs e)
        {
            string url = $"http://smartlockone.somee.com/puerta.aspx?id={idDetalle}&token=ABC123";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            qrImage = qrCode.GetGraphic(10);
            pictureBox1.Image = qrImage;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Imagen PNG|*.png";
            save.FileName = $"QR_Detalle_{idDetalle}.png";

            if (save.ShowDialog() == DialogResult.OK)
            {
                qrImage.Save(save.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Código QR guardado correctamente.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
