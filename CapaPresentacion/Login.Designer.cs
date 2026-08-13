namespace CapaPresentacion
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtcontrasena = new System.Windows.Forms.TextBox();
            this.txtid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btningresar = new FontAwesome.Sharp.IconButton();
            this.btncancelar = new FontAwesome.Sharp.IconButton();
            this.label6 = new System.Windows.Forms.Label();
            this.btneye1 = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 400);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(27, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 34);
            this.label2.TabIndex = 1;
            this.label2.Text = "SMARTLOCK";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(33, 127);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 185);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // txtcontrasena
            // 
            this.txtcontrasena.Font = new System.Drawing.Font("Consolas", 14F);
            this.txtcontrasena.Location = new System.Drawing.Point(281, 201);
            this.txtcontrasena.Name = "txtcontrasena";
            this.txtcontrasena.PasswordChar = '*';
            this.txtcontrasena.Size = new System.Drawing.Size(347, 40);
            this.txtcontrasena.TabIndex = 3;
            // 
            // txtid
            // 
            this.txtid.Font = new System.Drawing.Font("Consolas", 14F);
            this.txtid.Location = new System.Drawing.Point(281, 116);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(347, 40);
            this.txtid.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Bahnschrift", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(277, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Correo electrónico:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Bahnschrift", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(277, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Contraseña:";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Navy;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(215, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 400);
            this.label5.TabIndex = 7;
            // 
            // btningresar
            // 
            this.btningresar.BackColor = System.Drawing.Color.ForestGreen;
            this.btningresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btningresar.Font = new System.Drawing.Font("Bahnschrift", 10F);
            this.btningresar.ForeColor = System.Drawing.Color.White;
            this.btningresar.IconChar = FontAwesome.Sharp.IconChar.DoorOpen;
            this.btningresar.IconColor = System.Drawing.Color.White;
            this.btningresar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btningresar.IconSize = 36;
            this.btningresar.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btningresar.Location = new System.Drawing.Point(281, 313);
            this.btningresar.Name = "btningresar";
            this.btningresar.Size = new System.Drawing.Size(166, 47);
            this.btningresar.TabIndex = 8;
            this.btningresar.Text = "Ingresar";
            this.btningresar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btningresar.UseVisualStyleBackColor = false;
            this.btningresar.Click += new System.EventHandler(this.btningresar_Click);
            // 
            // btncancelar
            // 
            this.btncancelar.BackColor = System.Drawing.Color.Red;
            this.btncancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncancelar.Font = new System.Drawing.Font("Bahnschrift", 10F);
            this.btncancelar.ForeColor = System.Drawing.Color.White;
            this.btncancelar.IconChar = FontAwesome.Sharp.IconChar.CircleXmark;
            this.btncancelar.IconColor = System.Drawing.Color.White;
            this.btncancelar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btncancelar.IconSize = 35;
            this.btncancelar.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btncancelar.Location = new System.Drawing.Point(462, 313);
            this.btncancelar.Name = "btncancelar";
            this.btncancelar.Size = new System.Drawing.Size(166, 47);
            this.btncancelar.TabIndex = 9;
            this.btncancelar.Text = "Cancelar";
            this.btncancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncancelar.UseVisualStyleBackColor = false;
            this.btncancelar.Click += new System.EventHandler(this.btncancelar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Bahnschrift", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(277, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "INICIAR SESIÓN";
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
            this.btneye1.Location = new System.Drawing.Point(572, 201);
            this.btneye1.Name = "btneye1";
            this.btneye1.Size = new System.Drawing.Size(57, 29);
            this.btneye1.TabIndex = 37;
            this.btneye1.UseVisualStyleBackColor = false;
            this.btneye1.Click += new System.EventHandler(this.btneye1_Click);
            // 
            // Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(670, 400);
            this.Controls.Add(this.btneye1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btncancelar);
            this.Controls.Add(this.btningresar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.txtcontrasena);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 400);
            this.MinimumSize = new System.Drawing.Size(670, 400);
            this.Name = "Login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtcontrasena;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private FontAwesome.Sharp.IconButton btningresar;
        private FontAwesome.Sharp.IconButton btncancelar;
        private System.Windows.Forms.Label label6;
        private FontAwesome.Sharp.IconButton btneye1;
    }
}