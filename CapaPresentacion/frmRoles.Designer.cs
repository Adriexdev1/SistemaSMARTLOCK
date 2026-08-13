namespace CapaPresentacion
{
    partial class frmRoles
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnlimpiarbuscador = new FontAwesome.Sharp.IconButton();
            this.btnbuscar = new FontAwesome.Sharp.IconButton();
            this.txtbuscar = new System.Windows.Forms.TextBox();
            this.cmbfiltro = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtreg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.btnseleccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idrol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombrerol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcionrol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.permisorol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estadorol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registrorol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noestadorol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label11 = new System.Windows.Forms.Label();
            this.btneliminar = new FontAwesome.Sharp.IconButton();
            this.btnlimpiar = new FontAwesome.Sharp.IconButton();
            this.btnguardar = new FontAwesome.Sharp.IconButton();
            this.cboestado = new System.Windows.Forms.ComboBox();
            this.cbopermiso = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtdescripcion = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
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
            this.btnlimpiarbuscador.Location = new System.Drawing.Point(1366, 41);
            this.btnlimpiarbuscador.Name = "btnlimpiarbuscador";
            this.btnlimpiarbuscador.Size = new System.Drawing.Size(61, 36);
            this.btnlimpiarbuscador.TabIndex = 62;
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
            this.btnbuscar.Location = new System.Drawing.Point(1297, 41);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(61, 36);
            this.btnbuscar.TabIndex = 61;
            this.btnbuscar.UseVisualStyleBackColor = false;
            this.btnbuscar.Click += new System.EventHandler(this.btnbuscar_Click);
            // 
            // txtbuscar
            // 
            this.txtbuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbuscar.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbuscar.Location = new System.Drawing.Point(962, 45);
            this.txtbuscar.Name = "txtbuscar";
            this.txtbuscar.Size = new System.Drawing.Size(324, 36);
            this.txtbuscar.TabIndex = 60;
            // 
            // cmbfiltro
            // 
            this.cmbfiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbfiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbfiltro.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbfiltro.FormattingEnabled = true;
            this.cmbfiltro.Location = new System.Drawing.Point(797, 45);
            this.cmbfiltro.Name = "cmbfiltro";
            this.cmbfiltro.Size = new System.Drawing.Size(159, 36);
            this.cmbfiltro.TabIndex = 59;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(687, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 22);
            this.label13.TabIndex = 58;
            this.label13.Text = "Buscar por:";
            // 
            // txtreg
            // 
            this.txtreg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtreg.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtreg.Location = new System.Drawing.Point(30, 317);
            this.txtreg.Name = "txtreg";
            this.txtreg.ReadOnly = true;
            this.txtreg.Size = new System.Drawing.Size(303, 36);
            this.txtreg.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 22);
            this.label2.TabIndex = 56;
            this.label2.Text = "Fecha de registro:";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(378, 24);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(10);
            this.label12.Size = new System.Drawing.Size(1055, 72);
            this.label12.TabIndex = 55;
            this.label12.Text = "Lista de roles";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // txtnombre
            // 
            this.txtnombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtnombre.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.Location = new System.Drawing.Point(29, 74);
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.Size = new System.Drawing.Size(303, 36);
            this.txtnombre.TabIndex = 40;
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv1.ColumnHeadersHeight = 34;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnseleccion,
            this.idrol,
            this.nombrerol,
            this.descripcionrol,
            this.permisorol,
            this.estadorol,
            this.registrorol,
            this.noestadorol});
            this.dgv1.Location = new System.Drawing.Point(378, 115);
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.RowHeadersWidth = 62;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv1.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv1.RowTemplate.Height = 28;
            this.dgv1.Size = new System.Drawing.Size(1055, 614);
            this.dgv1.TabIndex = 54;
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
            // idrol
            // 
            this.idrol.FillWeight = 59.28729F;
            this.idrol.HeaderText = "ID";
            this.idrol.MinimumWidth = 10;
            this.idrol.Name = "idrol";
            this.idrol.ReadOnly = true;
            this.idrol.Width = 64;
            // 
            // nombrerol
            // 
            this.nombrerol.FillWeight = 120.9659F;
            this.nombrerol.HeaderText = "Nombre";
            this.nombrerol.MinimumWidth = 20;
            this.nombrerol.Name = "nombrerol";
            this.nombrerol.ReadOnly = true;
            this.nombrerol.Width = 108;
            // 
            // descripcionrol
            // 
            this.descripcionrol.FillWeight = 123.6637F;
            this.descripcionrol.HeaderText = "Descripción";
            this.descripcionrol.MinimumWidth = 20;
            this.descripcionrol.Name = "descripcionrol";
            this.descripcionrol.ReadOnly = true;
            this.descripcionrol.Width = 135;
            // 
            // permisorol
            // 
            this.permisorol.FillWeight = 99.92259F;
            this.permisorol.HeaderText = "Permiso";
            this.permisorol.MinimumWidth = 8;
            this.permisorol.Name = "permisorol";
            this.permisorol.ReadOnly = true;
            this.permisorol.Visible = false;
            this.permisorol.Width = 109;
            // 
            // estadorol
            // 
            this.estadorol.FillWeight = 142.0455F;
            this.estadorol.HeaderText = "Estado";
            this.estadorol.MinimumWidth = 25;
            this.estadorol.Name = "estadorol";
            this.estadorol.ReadOnly = true;
            this.estadorol.Width = 99;
            // 
            // registrorol
            // 
            this.registrorol.FillWeight = 99.92259F;
            this.registrorol.HeaderText = "Registro";
            this.registrorol.MinimumWidth = 8;
            this.registrorol.Name = "registrorol";
            this.registrorol.ReadOnly = true;
            this.registrorol.Width = 110;
            // 
            // noestadorol
            // 
            this.noestadorol.HeaderText = "No. Estado";
            this.noestadorol.MinimumWidth = 8;
            this.noestadorol.Name = "noestadorol";
            this.noestadorol.ReadOnly = true;
            this.noestadorol.Visible = false;
            this.noestadorol.Width = 127;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(24, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(158, 29);
            this.label11.TabIndex = 53;
            this.label11.Text = "Detalle de rol";
            // 
            // btneliminar
            // 
            this.btneliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btneliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btneliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btneliminar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            this.btneliminar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btneliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btneliminar.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btneliminar.ForeColor = System.Drawing.Color.White;
            this.btneliminar.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            this.btneliminar.IconColor = System.Drawing.Color.White;
            this.btneliminar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btneliminar.IconSize = 30;
            this.btneliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btneliminar.Location = new System.Drawing.Point(21, 692);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.Size = new System.Drawing.Size(311, 44);
            this.btneliminar.TabIndex = 52;
            this.btneliminar.Text = "Eliminar";
            this.btneliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btneliminar.UseVisualStyleBackColor = false;
            this.btneliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnlimpiar
            // 
            this.btnlimpiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnlimpiar.BackColor = System.Drawing.Color.Navy;
            this.btnlimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnlimpiar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btnlimpiar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnlimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlimpiar.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnlimpiar.ForeColor = System.Drawing.Color.White;
            this.btnlimpiar.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnlimpiar.IconColor = System.Drawing.Color.White;
            this.btnlimpiar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnlimpiar.IconSize = 30;
            this.btnlimpiar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnlimpiar.Location = new System.Drawing.Point(21, 642);
            this.btnlimpiar.Name = "btnlimpiar";
            this.btnlimpiar.Size = new System.Drawing.Size(311, 44);
            this.btnlimpiar.TabIndex = 51;
            this.btnlimpiar.Text = "Limpiar";
            this.btnlimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlimpiar.UseVisualStyleBackColor = false;
            this.btnlimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnguardar
            // 
            this.btnguardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnguardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnguardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnguardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btnguardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnguardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnguardar.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnguardar.ForeColor = System.Drawing.Color.White;
            this.btnguardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnguardar.IconColor = System.Drawing.Color.White;
            this.btnguardar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnguardar.IconSize = 30;
            this.btnguardar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnguardar.Location = new System.Drawing.Point(21, 592);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(311, 44);
            this.btnguardar.TabIndex = 50;
            this.btnguardar.Text = "Guardar";
            this.btnguardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnguardar.UseVisualStyleBackColor = false;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // cboestado
            // 
            this.cboestado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cboestado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboestado.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboestado.FormattingEnabled = true;
            this.cboestado.Location = new System.Drawing.Point(211, 250);
            this.cboestado.Name = "cboestado";
            this.cboestado.Size = new System.Drawing.Size(121, 36);
            this.cboestado.TabIndex = 49;
            // 
            // cbopermiso
            // 
            this.cbopermiso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cbopermiso.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbopermiso.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbopermiso.FormattingEnabled = true;
            this.cbopermiso.Location = new System.Drawing.Point(29, 250);
            this.cbopermiso.Name = "cbopermiso";
            this.cbopermiso.Size = new System.Drawing.Size(121, 36);
            this.cbopermiso.TabIndex = 48;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(207, 225);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 22);
            this.label10.TabIndex = 47;
            this.label10.Text = "Estado:";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(25, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 22);
            this.label9.TabIndex = 46;
            this.label9.Text = "Nivel de permiso:";
            // 
            // txtid
            // 
            this.txtid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtid.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtid.Location = new System.Drawing.Point(283, 12);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(58, 36);
            this.txtid.TabIndex = 39;
            this.txtid.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 22);
            this.label4.TabIndex = 35;
            this.label4.Text = "Descripción:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 22);
            this.label3.TabIndex = 34;
            this.label3.Text = "Nombre:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 749);
            this.label1.TabIndex = 33;
            // 
            // txtdescripcion
            // 
            this.txtdescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtdescripcion.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdescripcion.Location = new System.Drawing.Point(29, 140);
            this.txtdescripcion.MaximumSize = new System.Drawing.Size(303, 79);
            this.txtdescripcion.MinimumSize = new System.Drawing.Size(303, 79);
            this.txtdescripcion.Multiline = true;
            this.txtdescripcion.Name = "txtdescripcion";
            this.txtdescripcion.Size = new System.Drawing.Size(303, 79);
            this.txtdescripcion.TabIndex = 63;
            // 
            // frmRoles
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1453, 749);
            this.Controls.Add(this.txtdescripcion);
            this.Controls.Add(this.btnlimpiarbuscador);
            this.Controls.Add(this.btnbuscar);
            this.Controls.Add(this.txtbuscar);
            this.Controls.Add(this.cmbfiltro);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtreg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtnombre);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btneliminar);
            this.Controls.Add(this.btnlimpiar);
            this.Controls.Add(this.btnguardar);
            this.Controls.Add(this.cboestado);
            this.Controls.Add(this.cbopermiso);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "frmRoles";
            this.Text = "frmRoles";
            this.Load += new System.EventHandler(this.frmRoles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FontAwesome.Sharp.IconButton btnlimpiarbuscador;
        private FontAwesome.Sharp.IconButton btnbuscar;
        private System.Windows.Forms.TextBox txtbuscar;
        private System.Windows.Forms.ComboBox cmbfiltro;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtreg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.Label label11;
        private FontAwesome.Sharp.IconButton btneliminar;
        private FontAwesome.Sharp.IconButton btnlimpiar;
        private FontAwesome.Sharp.IconButton btnguardar;
        private System.Windows.Forms.ComboBox cboestado;
        private System.Windows.Forms.ComboBox cbopermiso;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtdescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn btnseleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn idrol;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombrerol;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcionrol;
        private System.Windows.Forms.DataGridViewTextBoxColumn permisorol;
        private System.Windows.Forms.DataGridViewTextBoxColumn estadorol;
        private System.Windows.Forms.DataGridViewTextBoxColumn registrorol;
        private System.Windows.Forms.DataGridViewTextBoxColumn noestadorol;
    }
}