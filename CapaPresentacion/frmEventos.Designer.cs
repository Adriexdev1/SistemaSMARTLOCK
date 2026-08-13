namespace CapaPresentacion
{
    partial class frmEventos
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
            this.txtfechainicio = new System.Windows.Forms.DateTimePicker();
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
            this.label11 = new System.Windows.Forms.Label();
            this.btneliminar = new FontAwesome.Sharp.IconButton();
            this.btnlimpiar = new FontAwesome.Sharp.IconButton();
            this.btnguardar = new FontAwesome.Sharp.IconButton();
            this.cboestado = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtdesc = new System.Windows.Forms.TextBox();
            this.txtid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txthorainicio = new System.Windows.Forms.DateTimePicker();
            this.txthorafin = new System.Windows.Forms.DateTimePicker();
            this.txtfechafin = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.btnusuarios = new FontAwesome.Sharp.IconButton();
            this.btnseleccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inicioevento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finevento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noestado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtfechainicio
            // 
            this.txtfechainicio.CustomFormat = "dddd, dd MMMM yyyy";
            this.txtfechainicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtfechainicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtfechainicio.Location = new System.Drawing.Point(25, 252);
            this.txtfechainicio.Name = "txtfechainicio";
            this.txtfechainicio.Size = new System.Drawing.Size(132, 30);
            this.txtfechainicio.TabIndex = 63;
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
            this.btnlimpiarbuscador.Location = new System.Drawing.Point(1357, 41);
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
            this.btnbuscar.Location = new System.Drawing.Point(1288, 41);
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
            this.txtbuscar.Location = new System.Drawing.Point(953, 45);
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
            this.cmbfiltro.Location = new System.Drawing.Point(788, 45);
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
            this.label13.Location = new System.Drawing.Point(678, 46);
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
            this.txtreg.Location = new System.Drawing.Point(24, 366);
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
            this.label2.Location = new System.Drawing.Point(21, 341);
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
            this.label12.Location = new System.Drawing.Point(377, 23);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(10);
            this.label12.Size = new System.Drawing.Size(1055, 72);
            this.label12.TabIndex = 55;
            this.label12.Text = "Lista de eventos";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtnombre
            // 
            this.txtnombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtnombre.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.Location = new System.Drawing.Point(24, 76);
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
            this.id,
            this.nombre,
            this.desc,
            this.inicioevento,
            this.finevento,
            this.estado,
            this.registro,
            this.noestado});
            this.dgv1.Location = new System.Drawing.Point(377, 115);
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
            this.dgv1.Size = new System.Drawing.Size(1055, 614);
            this.dgv1.TabIndex = 54;
            this.dgv1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellClick);
            this.dgv1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentClick);
            this.dgv1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv1_CellPainting);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(24, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 29);
            this.label11.TabIndex = 53;
            this.label11.Text = "Detalle de evento";
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
            this.btneliminar.Location = new System.Drawing.Point(24, 692);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.Size = new System.Drawing.Size(303, 44);
            this.btneliminar.TabIndex = 52;
            this.btneliminar.Text = "Eliminar";
            this.btneliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btneliminar.UseVisualStyleBackColor = false;
            this.btneliminar.Click += new System.EventHandler(this.btneliminar_Click);
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
            this.btnlimpiar.Location = new System.Drawing.Point(24, 642);
            this.btnlimpiar.Name = "btnlimpiar";
            this.btnlimpiar.Size = new System.Drawing.Size(303, 44);
            this.btnlimpiar.TabIndex = 51;
            this.btnlimpiar.Text = "Limpiar";
            this.btnlimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlimpiar.UseVisualStyleBackColor = false;
            this.btnlimpiar.Click += new System.EventHandler(this.btnlimpiar_Click);
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
            this.btnguardar.Location = new System.Drawing.Point(24, 592);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(303, 44);
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
            this.cboestado.Location = new System.Drawing.Point(25, 433);
            this.cboestado.Name = "cboestado";
            this.cboestado.Size = new System.Drawing.Size(302, 36);
            this.cboestado.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(21, 408);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 22);
            this.label10.TabIndex = 47;
            this.label10.Text = "Estado:";
            // 
            // txtdesc
            // 
            this.txtdesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtdesc.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdesc.Location = new System.Drawing.Point(25, 140);
            this.txtdesc.MaximumSize = new System.Drawing.Size(303, 79);
            this.txtdesc.MinimumSize = new System.Drawing.Size(303, 79);
            this.txtdesc.Multiline = true;
            this.txtdesc.Name = "txtdesc";
            this.txtdesc.Size = new System.Drawing.Size(303, 79);
            this.txtdesc.TabIndex = 41;
            // 
            // txtid
            // 
            this.txtid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtid.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtid.Location = new System.Drawing.Point(274, 12);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(58, 36);
            this.txtid.TabIndex = 39;
            this.txtid.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 22);
            this.label6.TabIndex = 37;
            this.label6.Text = "Fecha de inicio:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(21, 115);
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
            this.label3.Location = new System.Drawing.Point(20, 52);
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
            // txthorainicio
            // 
            this.txthorainicio.CustomFormat = "HH:mm:ss";
            this.txthorainicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txthorainicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txthorainicio.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txthorainicio.Location = new System.Drawing.Point(196, 252);
            this.txthorainicio.Name = "txthorainicio";
            this.txthorainicio.ShowUpDown = true;
            this.txthorainicio.Size = new System.Drawing.Size(133, 30);
            this.txthorainicio.TabIndex = 64;
            // 
            // txthorafin
            // 
            this.txthorafin.CustomFormat = "";
            this.txthorafin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txthorafin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txthorafin.Location = new System.Drawing.Point(197, 310);
            this.txthorafin.Name = "txthorafin";
            this.txthorafin.ShowUpDown = true;
            this.txthorafin.Size = new System.Drawing.Size(132, 30);
            this.txthorafin.TabIndex = 67;
            // 
            // txtfechafin
            // 
            this.txtfechafin.CustomFormat = "dddd, dd MMMM yyyy";
            this.txtfechafin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtfechafin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtfechafin.Location = new System.Drawing.Point(26, 310);
            this.txtfechafin.Name = "txtfechafin";
            this.txtfechafin.Size = new System.Drawing.Size(132, 30);
            this.txtfechafin.TabIndex = 66;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 22);
            this.label5.TabIndex = 65;
            this.label5.Text = "Fecha de fin:";
            // 
            // btnusuarios
            // 
            this.btnusuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnusuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnusuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnusuarios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btnusuarios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnusuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnusuarios.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnusuarios.ForeColor = System.Drawing.Color.White;
            this.btnusuarios.IconChar = FontAwesome.Sharp.IconChar.UserGear;
            this.btnusuarios.IconColor = System.Drawing.Color.White;
            this.btnusuarios.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnusuarios.IconSize = 30;
            this.btnusuarios.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnusuarios.Location = new System.Drawing.Point(25, 542);
            this.btnusuarios.Name = "btnusuarios";
            this.btnusuarios.Size = new System.Drawing.Size(303, 44);
            this.btnusuarios.TabIndex = 69;
            this.btnusuarios.Text = "Gestionar usuarios";
            this.btnusuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnusuarios.UseVisualStyleBackColor = false;
            this.btnusuarios.Click += new System.EventHandler(this.btnusuarios_Click);
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
            // desc
            // 
            this.desc.FillWeight = 123.6637F;
            this.desc.HeaderText = "Descripción";
            this.desc.MinimumWidth = 20;
            this.desc.Name = "desc";
            this.desc.ReadOnly = true;
            this.desc.Width = 135;
            // 
            // inicioevento
            // 
            this.inicioevento.FillWeight = 99.92259F;
            this.inicioevento.HeaderText = "Fecha inicio";
            this.inicioevento.MinimumWidth = 8;
            this.inicioevento.Name = "inicioevento";
            this.inicioevento.ReadOnly = true;
            this.inicioevento.Width = 135;
            // 
            // finevento
            // 
            this.finevento.FillWeight = 99.92259F;
            this.finevento.HeaderText = "Fecha fin";
            this.finevento.MinimumWidth = 8;
            this.finevento.Name = "finevento";
            this.finevento.ReadOnly = true;
            this.finevento.Width = 115;
            // 
            // estado
            // 
            this.estado.FillWeight = 142.0455F;
            this.estado.HeaderText = "Estado";
            this.estado.MinimumWidth = 25;
            this.estado.Name = "estado";
            this.estado.ReadOnly = true;
            this.estado.Width = 99;
            // 
            // registro
            // 
            this.registro.FillWeight = 99.92259F;
            this.registro.HeaderText = "Registro";
            this.registro.MinimumWidth = 8;
            this.registro.Name = "registro";
            this.registro.ReadOnly = true;
            this.registro.Width = 110;
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
            // frmEventos
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1453, 749);
            this.Controls.Add(this.btnusuarios);
            this.Controls.Add(this.txthorafin);
            this.Controls.Add(this.txtfechafin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txthorainicio);
            this.Controls.Add(this.txtfechainicio);
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
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtdesc);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "frmEventos";
            this.Text = "frmEventos";
            this.Load += new System.EventHandler(this.frmEventos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker txtfechainicio;
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtdesc;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txthorainicio;
        private System.Windows.Forms.DateTimePicker txthorafin;
        private System.Windows.Forms.DateTimePicker txtfechafin;
        private System.Windows.Forms.Label label5;
        private FontAwesome.Sharp.IconButton btnusuarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn btnseleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn inicioevento;
        private System.Windows.Forms.DataGridViewTextBoxColumn finevento;
        private System.Windows.Forms.DataGridViewTextBoxColumn estado;
        private System.Windows.Forms.DataGridViewTextBoxColumn registro;
        private System.Windows.Forms.DataGridViewTextBoxColumn noestado;
    }
}