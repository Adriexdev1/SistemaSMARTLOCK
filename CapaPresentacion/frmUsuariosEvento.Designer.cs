namespace CapaPresentacion
{
    partial class frmUsuariosEvento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbuscar = new System.Windows.Forms.TextBox();
            this.cmbfiltro = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.btnseleccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idusuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombreusuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.correousuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telusuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rolusuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.claveusuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fechanacimientousuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estadousuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registrousuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idrol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noestado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv2 = new System.Windows.Forms.DataGridView();
            this.btneliminar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iddetalle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acceso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnlimpiarbuscador = new FontAwesome.Sharp.IconButton();
            this.btnbuscar = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(978, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuarios del evento";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(452, 436);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(505, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(446, 436);
            this.label3.TabIndex = 2;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(39, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(294, 38);
            this.label4.TabIndex = 3;
            this.label4.Text = "Agregar usuario";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtbuscar
            // 
            this.txtbuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbuscar.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbuscar.Location = new System.Drawing.Point(44, 131);
            this.txtbuscar.Name = "txtbuscar";
            this.txtbuscar.Size = new System.Drawing.Size(268, 36);
            this.txtbuscar.TabIndex = 34;
            // 
            // cmbfiltro
            // 
            this.cmbfiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbfiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbfiltro.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbfiltro.FormattingEnabled = true;
            this.cmbfiltro.Location = new System.Drawing.Point(187, 179);
            this.cmbfiltro.Name = "cmbfiltro";
            this.cmbfiltro.Size = new System.Drawing.Size(273, 36);
            this.cmbfiltro.TabIndex = 33;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(40, 186);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 22);
            this.label13.TabIndex = 32;
            this.label13.Text = "Buscar por:";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(516, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(333, 38);
            this.label5.TabIndex = 39;
            this.label5.Text = "Lista de usuarios del evento";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv1.ColumnHeadersHeight = 34;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnseleccion,
            this.idusuario,
            this.nombreusuario,
            this.correousuario,
            this.telusuario,
            this.rolusuario,
            this.claveusuario,
            this.fechanacimientousuario,
            this.estadousuario,
            this.registrousuario,
            this.idrol,
            this.noestado});
            this.dgv1.Location = new System.Drawing.Point(44, 239);
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.RowHeadersWidth = 62;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv1.RowTemplate.Height = 28;
            this.dgv1.Size = new System.Drawing.Size(416, 260);
            this.dgv1.TabIndex = 40;
            this.dgv1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellClick);
            this.dgv1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv1_CellPainting);
            // 
            // btnseleccion
            // 
            this.btnseleccion.FillWeight = 54.42485F;
            this.btnseleccion.HeaderText = "";
            this.btnseleccion.MinimumWidth = 10;
            this.btnseleccion.Name = "btnseleccion";
            this.btnseleccion.ReadOnly = true;
            this.btnseleccion.Width = 33;
            // 
            // idusuario
            // 
            this.idusuario.FillWeight = 59.28729F;
            this.idusuario.HeaderText = "ID";
            this.idusuario.MinimumWidth = 10;
            this.idusuario.Name = "idusuario";
            this.idusuario.ReadOnly = true;
            this.idusuario.Width = 64;
            // 
            // nombreusuario
            // 
            this.nombreusuario.FillWeight = 120.9659F;
            this.nombreusuario.HeaderText = "Nombre";
            this.nombreusuario.MinimumWidth = 20;
            this.nombreusuario.Name = "nombreusuario";
            this.nombreusuario.ReadOnly = true;
            this.nombreusuario.Width = 108;
            // 
            // correousuario
            // 
            this.correousuario.FillWeight = 123.6637F;
            this.correousuario.HeaderText = "Correo";
            this.correousuario.MinimumWidth = 20;
            this.correousuario.Name = "correousuario";
            this.correousuario.ReadOnly = true;
            // 
            // telusuario
            // 
            this.telusuario.FillWeight = 99.92259F;
            this.telusuario.HeaderText = "Teléfono";
            this.telusuario.MinimumWidth = 8;
            this.telusuario.Name = "telusuario";
            this.telusuario.ReadOnly = true;
            this.telusuario.Width = 111;
            // 
            // rolusuario
            // 
            this.rolusuario.FillWeight = 99.92259F;
            this.rolusuario.HeaderText = "Rol";
            this.rolusuario.MinimumWidth = 8;
            this.rolusuario.Name = "rolusuario";
            this.rolusuario.ReadOnly = true;
            this.rolusuario.Width = 73;
            // 
            // claveusuario
            // 
            this.claveusuario.FillWeight = 99.92259F;
            this.claveusuario.HeaderText = "Clave";
            this.claveusuario.MinimumWidth = 8;
            this.claveusuario.Name = "claveusuario";
            this.claveusuario.ReadOnly = true;
            this.claveusuario.Visible = false;
            this.claveusuario.Width = 90;
            // 
            // fechanacimientousuario
            // 
            this.fechanacimientousuario.FillWeight = 99.92259F;
            this.fechanacimientousuario.HeaderText = "Fecha";
            this.fechanacimientousuario.MinimumWidth = 8;
            this.fechanacimientousuario.Name = "fechanacimientousuario";
            this.fechanacimientousuario.ReadOnly = true;
            this.fechanacimientousuario.Width = 93;
            // 
            // estadousuario
            // 
            this.estadousuario.FillWeight = 142.0455F;
            this.estadousuario.HeaderText = "Estado";
            this.estadousuario.MinimumWidth = 25;
            this.estadousuario.Name = "estadousuario";
            this.estadousuario.ReadOnly = true;
            this.estadousuario.Width = 99;
            // 
            // registrousuario
            // 
            this.registrousuario.FillWeight = 99.92259F;
            this.registrousuario.HeaderText = "Registro";
            this.registrousuario.MinimumWidth = 8;
            this.registrousuario.Name = "registrousuario";
            this.registrousuario.ReadOnly = true;
            this.registrousuario.Width = 110;
            // 
            // idrol
            // 
            this.idrol.HeaderText = "ID Rol";
            this.idrol.MinimumWidth = 8;
            this.idrol.Name = "idrol";
            this.idrol.ReadOnly = true;
            this.idrol.Visible = false;
            this.idrol.Width = 92;
            // 
            // noestado
            // 
            this.noestado.HeaderText = "No. Estado";
            this.noestado.MinimumWidth = 8;
            this.noestado.Name = "noestado";
            this.noestado.ReadOnly = true;
            this.noestado.Visible = false;
            this.noestado.Width = 127;
            // 
            // dgv2
            // 
            this.dgv2.AllowUserToAddRows = false;
            this.dgv2.AllowUserToDeleteRows = false;
            this.dgv2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv2.ColumnHeadersHeight = 34;
            this.dgv2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btneliminar,
            this.id,
            this.nombre,
            this.iddetalle,
            this.acceso,
            this.qr});
            this.dgv2.Location = new System.Drawing.Point(521, 131);
            this.dgv2.Name = "dgv2";
            this.dgv2.ReadOnly = true;
            this.dgv2.RowHeadersVisible = false;
            this.dgv2.RowHeadersWidth = 62;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv2.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv2.RowTemplate.Height = 28;
            this.dgv2.Size = new System.Drawing.Size(409, 368);
            this.dgv2.TabIndex = 41;
            this.dgv2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv2_CellClick);
            this.dgv2.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv2_CellPainting);
            // 
            // btneliminar
            // 
            this.btneliminar.FillWeight = 54.42485F;
            this.btneliminar.HeaderText = "";
            this.btneliminar.MinimumWidth = 10;
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.ReadOnly = true;
            this.btneliminar.Width = 33;
            // 
            // id
            // 
            this.id.FillWeight = 59.28729F;
            this.id.HeaderText = "ID";
            this.id.MinimumWidth = 10;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 64;
            // 
            // nombre
            // 
            this.nombre.FillWeight = 120.9659F;
            this.nombre.HeaderText = "Nombre";
            this.nombre.MinimumWidth = 20;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            this.nombre.Width = 108;
            // 
            // iddetalle
            // 
            this.iddetalle.HeaderText = "ID Detalle";
            this.iddetalle.MinimumWidth = 8;
            this.iddetalle.Name = "iddetalle";
            this.iddetalle.ReadOnly = true;
            this.iddetalle.Width = 121;
            // 
            // acceso
            // 
            this.acceso.HeaderText = "Acceso";
            this.acceso.MinimumWidth = 8;
            this.acceso.Name = "acceso";
            this.acceso.ReadOnly = true;
            this.acceso.Width = 101;
            // 
            // qr
            // 
            this.qr.HeaderText = "QR";
            this.qr.MinimumWidth = 8;
            this.qr.Name = "qr";
            this.qr.ReadOnly = true;
            this.qr.Width = 70;
            // 
            // btnlimpiarbuscador
            // 
            this.btnlimpiarbuscador.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnlimpiarbuscador.BackColor = System.Drawing.Color.Navy;
            this.btnlimpiarbuscador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnlimpiarbuscador.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnlimpiarbuscador.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnlimpiarbuscador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlimpiarbuscador.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnlimpiarbuscador.ForeColor = System.Drawing.Color.White;
            this.btnlimpiarbuscador.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnlimpiarbuscador.IconColor = System.Drawing.Color.White;
            this.btnlimpiarbuscador.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnlimpiarbuscador.IconSize = 30;
            this.btnlimpiarbuscador.Location = new System.Drawing.Point(399, 131);
            this.btnlimpiarbuscador.Name = "btnlimpiarbuscador";
            this.btnlimpiarbuscador.Size = new System.Drawing.Size(61, 36);
            this.btnlimpiarbuscador.TabIndex = 36;
            this.btnlimpiarbuscador.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlimpiarbuscador.UseVisualStyleBackColor = false;
            this.btnlimpiarbuscador.Click += new System.EventHandler(this.btnlimpiarbuscador_Click);
            // 
            // btnbuscar
            // 
            this.btnbuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnbuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnbuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnbuscar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btnbuscar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnbuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnbuscar.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbuscar.ForeColor = System.Drawing.Color.White;
            this.btnbuscar.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnbuscar.IconColor = System.Drawing.Color.White;
            this.btnbuscar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnbuscar.IconSize = 30;
            this.btnbuscar.Location = new System.Drawing.Point(328, 131);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(61, 36);
            this.btnbuscar.TabIndex = 35;
            this.btnbuscar.UseVisualStyleBackColor = false;
            this.btnbuscar.Click += new System.EventHandler(this.btnbuscar_Click);
            // 
            // frmUsuariosEvento
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(978, 544);
            this.Controls.Add(this.dgv2);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnlimpiarbuscador);
            this.Controls.Add(this.btnbuscar);
            this.Controls.Add(this.txtbuscar);
            this.Controls.Add(this.cmbfiltro);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(1000, 600);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "frmUsuariosEvento";
            this.Load += new System.EventHandler(this.frmUsuariosEvento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private FontAwesome.Sharp.IconButton btnlimpiarbuscador;
        private FontAwesome.Sharp.IconButton btnbuscar;
        private System.Windows.Forms.TextBox txtbuscar;
        private System.Windows.Forms.ComboBox cmbfiltro;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.DataGridViewTextBoxColumn btnseleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn idusuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreusuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn correousuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn telusuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn rolusuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn claveusuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn fechanacimientousuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn estadousuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn registrousuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn idrol;
        private System.Windows.Forms.DataGridViewTextBoxColumn noestado;
        private System.Windows.Forms.DataGridView dgv2;
        private System.Windows.Forms.DataGridViewTextBoxColumn btneliminar;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn iddetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn acceso;
        private System.Windows.Forms.DataGridViewTextBoxColumn qr;
    }
}