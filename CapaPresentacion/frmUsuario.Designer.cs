namespace CapaPresentacion
{
    partial class frmUsuario
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtid = new System.Windows.Forms.TextBox();
            this.txtcorreo = new System.Windows.Forms.TextBox();
            this.txttel = new System.Windows.Forms.TextBox();
            this.txtcontra = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtconf = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cborol = new System.Windows.Forms.ComboBox();
            this.cboestado = new System.Windows.Forms.ComboBox();
            this.btnguardar = new FontAwesome.Sharp.IconButton();
            this.btnlimpiar = new FontAwesome.Sharp.IconButton();
            this.btneliminar = new FontAwesome.Sharp.IconButton();
            this.label11 = new System.Windows.Forms.Label();
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
            this.label12 = new System.Windows.Forms.Label();
            this.txtreg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbfiltro = new System.Windows.Forms.ComboBox();
            this.txtbuscar = new System.Windows.Forms.TextBox();
            this.btnbuscar = new FontAwesome.Sharp.IconButton();
            this.btnlimpiarbuscador = new FontAwesome.Sharp.IconButton();
            this.txtfecha = new System.Windows.Forms.DateTimePicker();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.btneye1 = new FontAwesome.Sharp.IconButton();
            this.btneye2 = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 749);
            this.label1.TabIndex = 0;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nombre:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "Correo:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(24, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 22);
            this.label5.TabIndex = 4;
            this.label5.Text = "Teléfono:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(23, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 22);
            this.label6.TabIndex = 5;
            this.label6.Text = "Fecha de nacimiento:";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(24, 425);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 22);
            this.label7.TabIndex = 6;
            this.label7.Text = "Contraseña:";
            // 
            // txtid
            // 
            this.txtid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtid.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtid.Location = new System.Drawing.Point(272, 12);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(58, 36);
            this.txtid.TabIndex = 7;
            this.txtid.Visible = false;
            // 
            // txtcorreo
            // 
            this.txtcorreo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtcorreo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcorreo.Location = new System.Drawing.Point(28, 196);
            this.txtcorreo.Name = "txtcorreo";
            this.txtcorreo.Size = new System.Drawing.Size(303, 36);
            this.txtcorreo.TabIndex = 10;
            // 
            // txttel
            // 
            this.txttel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txttel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttel.Location = new System.Drawing.Point(28, 257);
            this.txttel.Name = "txttel";
            this.txttel.Size = new System.Drawing.Size(303, 36);
            this.txttel.TabIndex = 11;
            // 
            // txtcontra
            // 
            this.txtcontra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtcontra.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcontra.Location = new System.Drawing.Point(28, 449);
            this.txtcontra.Name = "txtcontra";
            this.txtcontra.PasswordChar = '*';
            this.txtcontra.Size = new System.Drawing.Size(303, 36);
            this.txtcontra.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(23, 488);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(194, 22);
            this.label8.TabIndex = 13;
            this.label8.Text = "Confirmar contraseña:";
            // 
            // txtconf
            // 
            this.txtconf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtconf.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtconf.Location = new System.Drawing.Point(27, 513);
            this.txtconf.Name = "txtconf";
            this.txtconf.PasswordChar = '*';
            this.txtconf.Size = new System.Drawing.Size(303, 36);
            this.txtconf.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(22, 295);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 22);
            this.label9.TabIndex = 15;
            this.label9.Text = "Rol:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(204, 295);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 22);
            this.label10.TabIndex = 16;
            this.label10.Text = "Estado:";
            // 
            // cborol
            // 
            this.cborol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cborol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cborol.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cborol.FormattingEnabled = true;
            this.cborol.Location = new System.Drawing.Point(26, 320);
            this.cborol.Name = "cborol";
            this.cborol.Size = new System.Drawing.Size(121, 36);
            this.cborol.TabIndex = 17;
            this.cborol.SelectedIndexChanged += new System.EventHandler(this.cborol_SelectedIndexChanged);
            // 
            // cboestado
            // 
            this.cboestado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cboestado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboestado.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboestado.FormattingEnabled = true;
            this.cboestado.Location = new System.Drawing.Point(208, 320);
            this.cboestado.Name = "cboestado";
            this.cboestado.Size = new System.Drawing.Size(121, 36);
            this.cboestado.TabIndex = 18;
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
            this.btnguardar.Location = new System.Drawing.Point(19, 592);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(311, 44);
            this.btnguardar.TabIndex = 19;
            this.btnguardar.Text = "Guardar";
            this.btnguardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnguardar.UseVisualStyleBackColor = false;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
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
            this.btnlimpiar.Location = new System.Drawing.Point(19, 642);
            this.btnlimpiar.Name = "btnlimpiar";
            this.btnlimpiar.Size = new System.Drawing.Size(311, 44);
            this.btnlimpiar.TabIndex = 20;
            this.btnlimpiar.Text = "Limpiar";
            this.btnlimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlimpiar.UseVisualStyleBackColor = false;
            this.btnlimpiar.Click += new System.EventHandler(this.btnlimpiar_Click);
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
            this.btneliminar.Location = new System.Drawing.Point(19, 692);
            this.btneliminar.Name = "btneliminar";
            this.btneliminar.Size = new System.Drawing.Size(311, 44);
            this.btneliminar.TabIndex = 21;
            this.btneliminar.Text = "Eliminar";
            this.btneliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btneliminar.UseVisualStyleBackColor = false;
            this.btneliminar.Click += new System.EventHandler(this.btneliminar_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(22, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(208, 29);
            this.label11.TabIndex = 22;
            this.label11.Text = "Detalle de usuario";
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
            this.dgv1.Location = new System.Drawing.Point(375, 115);
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
            this.dgv1.TabIndex = 23;
            this.dgv1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellClick);
            this.dgv1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentClick);
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
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(375, 23);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(10);
            this.label12.Size = new System.Drawing.Size(1055, 72);
            this.label12.TabIndex = 24;
            this.label12.Text = "Lista de usuarios";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // txtreg
            // 
            this.txtreg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtreg.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtreg.Location = new System.Drawing.Point(27, 385);
            this.txtreg.Name = "txtreg";
            this.txtreg.ReadOnly = true;
            this.txtreg.Size = new System.Drawing.Size(303, 36);
            this.txtreg.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 22);
            this.label2.TabIndex = 25;
            this.label2.Text = "Fecha de registro:";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(676, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 22);
            this.label13.TabIndex = 27;
            this.label13.Text = "Buscar por:";
            // 
            // cmbfiltro
            // 
            this.cmbfiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbfiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbfiltro.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbfiltro.FormattingEnabled = true;
            this.cmbfiltro.Location = new System.Drawing.Point(786, 45);
            this.cmbfiltro.Name = "cmbfiltro";
            this.cmbfiltro.Size = new System.Drawing.Size(159, 36);
            this.cmbfiltro.TabIndex = 28;
            // 
            // txtbuscar
            // 
            this.txtbuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbuscar.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbuscar.Location = new System.Drawing.Point(951, 45);
            this.txtbuscar.Name = "txtbuscar";
            this.txtbuscar.Size = new System.Drawing.Size(324, 36);
            this.txtbuscar.TabIndex = 29;
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
            this.btnbuscar.Location = new System.Drawing.Point(1286, 41);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(61, 36);
            this.btnbuscar.TabIndex = 30;
            this.btnbuscar.UseVisualStyleBackColor = false;
            this.btnbuscar.Click += new System.EventHandler(this.btnbuscar_Click);
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
            this.btnlimpiarbuscador.Location = new System.Drawing.Point(1355, 41);
            this.btnlimpiarbuscador.Name = "btnlimpiarbuscador";
            this.btnlimpiarbuscador.Size = new System.Drawing.Size(61, 36);
            this.btnlimpiarbuscador.TabIndex = 31;
            this.btnlimpiarbuscador.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlimpiarbuscador.UseVisualStyleBackColor = false;
            this.btnlimpiarbuscador.Click += new System.EventHandler(this.btnlimpiarbuscador_Click);
            // 
            // txtfecha
            // 
            this.txtfecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtfecha.Location = new System.Drawing.Point(28, 142);
            this.txtfecha.Name = "txtfecha";
            this.txtfecha.Size = new System.Drawing.Size(304, 30);
            this.txtfecha.TabIndex = 32;
            // 
            // txtnombre
            // 
            this.txtnombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtnombre.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.Location = new System.Drawing.Point(27, 74);
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.Size = new System.Drawing.Size(303, 36);
            this.txtnombre.TabIndex = 8;
            // 
            // btneye1
            // 
            this.btneye1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btneye1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btneye1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btneye1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btneye1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btneye1.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btneye1.ForeColor = System.Drawing.Color.White;
            this.btneye1.IconChar = FontAwesome.Sharp.IconChar.Eye;
            this.btneye1.IconColor = System.Drawing.Color.White;
            this.btneye1.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btneye1.IconSize = 30;
            this.btneye1.Location = new System.Drawing.Point(274, 448);
            this.btneye1.Name = "btneye1";
            this.btneye1.Size = new System.Drawing.Size(57, 29);
            this.btneye1.TabIndex = 36;
            this.btneye1.UseVisualStyleBackColor = false;
            this.btneye1.Click += new System.EventHandler(this.btneye1_Click);
            // 
            // btneye2
            // 
            this.btneye2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btneye2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btneye2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btneye2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btneye2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btneye2.Font = new System.Drawing.Font("Bahnschrift", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btneye2.ForeColor = System.Drawing.Color.White;
            this.btneye2.IconChar = FontAwesome.Sharp.IconChar.Eye;
            this.btneye2.IconColor = System.Drawing.Color.White;
            this.btneye2.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btneye2.IconSize = 30;
            this.btneye2.Location = new System.Drawing.Point(274, 513);
            this.btneye2.Name = "btneye2";
            this.btneye2.Size = new System.Drawing.Size(57, 29);
            this.btneye2.TabIndex = 37;
            this.btneye2.UseVisualStyleBackColor = false;
            this.btneye2.Click += new System.EventHandler(this.btneye2_Click);
            // 
            // frmUsuario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1453, 749);
            this.Controls.Add(this.btneye2);
            this.Controls.Add(this.btneye1);
            this.Controls.Add(this.txtfecha);
            this.Controls.Add(this.btnlimpiarbuscador);
            this.Controls.Add(this.btnbuscar);
            this.Controls.Add(this.txtbuscar);
            this.Controls.Add(this.cmbfiltro);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtreg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btneliminar);
            this.Controls.Add(this.btnlimpiar);
            this.Controls.Add(this.btnguardar);
            this.Controls.Add(this.cboestado);
            this.Controls.Add(this.cborol);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtconf);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtcontra);
            this.Controls.Add(this.txttel);
            this.Controls.Add(this.txtcorreo);
            this.Controls.Add(this.txtnombre);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "frmUsuario";
            this.Text = "frmUsuario";
            this.Load += new System.EventHandler(this.frmUsuario_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.TextBox txtcorreo;
        private System.Windows.Forms.TextBox txttel;
        private System.Windows.Forms.TextBox txtcontra;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtconf;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cborol;
        private System.Windows.Forms.ComboBox cboestado;
        private FontAwesome.Sharp.IconButton btnguardar;
        private FontAwesome.Sharp.IconButton btnlimpiar;
        private FontAwesome.Sharp.IconButton btneliminar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtreg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbfiltro;
        private System.Windows.Forms.TextBox txtbuscar;
        private FontAwesome.Sharp.IconButton btnbuscar;
        private FontAwesome.Sharp.IconButton btnlimpiarbuscador;
        private System.Windows.Forms.DateTimePicker txtfecha;
        private System.Windows.Forms.TextBox txtnombre;
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
        private FontAwesome.Sharp.IconButton btneye1;
        private FontAwesome.Sharp.IconButton btneye2;
    }
}