using FontAwesome.Sharp;

namespace CapaPresentacion
{
    partial class Inicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.encabezado = new System.Windows.Forms.MenuStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.contenedor = new System.Windows.Forms.Panel();
            this.menu_roles = new FontAwesome.Sharp.IconButton();
            this.menu_ajustes = new FontAwesome.Sharp.IconButton();
            this.label2 = new System.Windows.Forms.Label();
            this.nomUsuario = new System.Windows.Forms.Label();
            this.menu_acercade = new FontAwesome.Sharp.IconButton();
            this.menu_eventos = new FontAwesome.Sharp.IconButton();
            this.menu_usuarios = new FontAwesome.Sharp.IconButton();
            this.btnexit = new FontAwesome.Sharp.IconButton();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.menu.Location = new System.Drawing.Point(0, 100);
            this.menu.MinimumSize = new System.Drawing.Size(180, 544);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.menu.Size = new System.Drawing.Size(180, 744);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(9, 4);
            // 
            // encabezado
            // 
            this.encabezado.AutoSize = false;
            this.encabezado.BackColor = System.Drawing.Color.Navy;
            this.encabezado.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.encabezado.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.encabezado.Location = new System.Drawing.Point(0, 0);
            this.encabezado.Name = "encabezado";
            this.encabezado.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.encabezado.Size = new System.Drawing.Size(1628, 100);
            this.encabezado.TabIndex = 1;
            this.encabezado.Text = "menuStrip2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Navy;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 60);
            this.label1.TabIndex = 2;
            this.label1.Text = "SMARTLOCK";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contenedor
            // 
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(180, 100);
            this.contenedor.Name = "contenedor";
            this.contenedor.Size = new System.Drawing.Size(1448, 744);
            this.contenedor.TabIndex = 3;
            // 
            // menu_roles
            // 
            this.menu_roles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_roles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_roles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menu_roles.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_roles.FlatAppearance.BorderSize = 0;
            this.menu_roles.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.menu_roles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu_roles.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.menu_roles.ForeColor = System.Drawing.Color.White;
            this.menu_roles.IconChar = FontAwesome.Sharp.IconChar.AddressCard;
            this.menu_roles.IconColor = System.Drawing.Color.White;
            this.menu_roles.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menu_roles.IconSize = 50;
            this.menu_roles.Location = new System.Drawing.Point(0, 321);
            this.menu_roles.MaximumSize = new System.Drawing.Size(180, 100);
            this.menu_roles.MinimumSize = new System.Drawing.Size(180, 100);
            this.menu_roles.Name = "menu_roles";
            this.menu_roles.Size = new System.Drawing.Size(180, 100);
            this.menu_roles.TabIndex = 8;
            this.menu_roles.Text = "Roles";
            this.menu_roles.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menu_roles.UseVisualStyleBackColor = false;
            this.menu_roles.Click += new System.EventHandler(this.menu_roles_Click);
            // 
            // menu_ajustes
            // 
            this.menu_ajustes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_ajustes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_ajustes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menu_ajustes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_ajustes.FlatAppearance.BorderSize = 0;
            this.menu_ajustes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.menu_ajustes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu_ajustes.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.menu_ajustes.ForeColor = System.Drawing.Color.White;
            this.menu_ajustes.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.menu_ajustes.IconColor = System.Drawing.Color.White;
            this.menu_ajustes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menu_ajustes.IconSize = 50;
            this.menu_ajustes.Location = new System.Drawing.Point(0, 627);
            this.menu_ajustes.MaximumSize = new System.Drawing.Size(180, 100);
            this.menu_ajustes.MinimumSize = new System.Drawing.Size(180, 100);
            this.menu_ajustes.Name = "menu_ajustes";
            this.menu_ajustes.Size = new System.Drawing.Size(180, 100);
            this.menu_ajustes.TabIndex = 7;
            this.menu_ajustes.Text = "Ajustes";
            this.menu_ajustes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menu_ajustes.UseVisualStyleBackColor = false;
            this.menu_ajustes.Click += new System.EventHandler(this.menu_ajustes_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Navy;
            this.label2.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(21, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "S I S T E M A";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nomUsuario
            // 
            this.nomUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nomUsuario.BackColor = System.Drawing.Color.Navy;
            this.nomUsuario.Font = new System.Drawing.Font("Bahnschrift", 12F);
            this.nomUsuario.ForeColor = System.Drawing.Color.White;
            this.nomUsuario.Location = new System.Drawing.Point(1130, 33);
            this.nomUsuario.MaximumSize = new System.Drawing.Size(400, 49);
            this.nomUsuario.Name = "nomUsuario";
            this.nomUsuario.Size = new System.Drawing.Size(400, 32);
            this.nomUsuario.TabIndex = 6;
            this.nomUsuario.Text = "USUARIO: ADMINISTRADOR";
            this.nomUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nomUsuario.UseMnemonic = false;
            // 
            // menu_acercade
            // 
            this.menu_acercade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_acercade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_acercade.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menu_acercade.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_acercade.FlatAppearance.BorderSize = 0;
            this.menu_acercade.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.menu_acercade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu_acercade.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.menu_acercade.ForeColor = System.Drawing.Color.White;
            this.menu_acercade.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            this.menu_acercade.IconColor = System.Drawing.Color.White;
            this.menu_acercade.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menu_acercade.IconSize = 50;
            this.menu_acercade.Location = new System.Drawing.Point(0, 733);
            this.menu_acercade.MaximumSize = new System.Drawing.Size(180, 100);
            this.menu_acercade.MinimumSize = new System.Drawing.Size(180, 100);
            this.menu_acercade.Name = "menu_acercade";
            this.menu_acercade.Size = new System.Drawing.Size(180, 100);
            this.menu_acercade.TabIndex = 0;
            this.menu_acercade.Text = "Acerca de";
            this.menu_acercade.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menu_acercade.UseVisualStyleBackColor = false;
            this.menu_acercade.Click += new System.EventHandler(this.menu_acercade_Click);
            // 
            // menu_eventos
            // 
            this.menu_eventos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_eventos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_eventos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menu_eventos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_eventos.FlatAppearance.BorderSize = 0;
            this.menu_eventos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.menu_eventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu_eventos.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.menu_eventos.ForeColor = System.Drawing.Color.White;
            this.menu_eventos.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
            this.menu_eventos.IconColor = System.Drawing.Color.White;
            this.menu_eventos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menu_eventos.IconSize = 50;
            this.menu_eventos.Location = new System.Drawing.Point(0, 215);
            this.menu_eventos.MaximumSize = new System.Drawing.Size(180, 100);
            this.menu_eventos.MinimumSize = new System.Drawing.Size(180, 100);
            this.menu_eventos.Name = "menu_eventos";
            this.menu_eventos.Size = new System.Drawing.Size(180, 100);
            this.menu_eventos.TabIndex = 9;
            this.menu_eventos.Text = "Eventos";
            this.menu_eventos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menu_eventos.UseVisualStyleBackColor = false;
            this.menu_eventos.Click += new System.EventHandler(this.menu_eventos_Click);
            // 
            // menu_usuarios
            // 
            this.menu_usuarios.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu_usuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_usuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menu_usuarios.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.menu_usuarios.FlatAppearance.BorderSize = 0;
            this.menu_usuarios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.menu_usuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu_usuarios.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.menu_usuarios.ForeColor = System.Drawing.Color.White;
            this.menu_usuarios.IconChar = FontAwesome.Sharp.IconChar.UserAlt;
            this.menu_usuarios.IconColor = System.Drawing.Color.White;
            this.menu_usuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menu_usuarios.IconSize = 50;
            this.menu_usuarios.Location = new System.Drawing.Point(0, 109);
            this.menu_usuarios.MaximumSize = new System.Drawing.Size(180, 100);
            this.menu_usuarios.MinimumSize = new System.Drawing.Size(180, 100);
            this.menu_usuarios.Name = "menu_usuarios";
            this.menu_usuarios.Size = new System.Drawing.Size(180, 100);
            this.menu_usuarios.TabIndex = 10;
            this.menu_usuarios.Text = "Usuarios";
            this.menu_usuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menu_usuarios.UseVisualStyleBackColor = false;
            this.menu_usuarios.Click += new System.EventHandler(this.menu_usuarios_Click);
            // 
            // btnexit
            // 
            this.btnexit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnexit.BackColor = System.Drawing.Color.Navy;
            this.btnexit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnexit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnexit.FlatAppearance.BorderSize = 0;
            this.btnexit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnexit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnexit.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Bold);
            this.btnexit.ForeColor = System.Drawing.Color.White;
            this.btnexit.IconChar = FontAwesome.Sharp.IconChar.SignOut;
            this.btnexit.IconColor = System.Drawing.Color.White;
            this.btnexit.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnexit.IconSize = 50;
            this.btnexit.Location = new System.Drawing.Point(1536, 12);
            this.btnexit.MaximumSize = new System.Drawing.Size(80, 80);
            this.btnexit.MinimumSize = new System.Drawing.Size(80, 80);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(80, 80);
            this.btnexit.TabIndex = 11;
            this.btnexit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnexit.UseVisualStyleBackColor = false;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // Inicio
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1628, 844);
            this.Controls.Add(this.btnexit);
            this.Controls.Add(this.menu_ajustes);
            this.Controls.Add(this.menu_roles);
            this.Controls.Add(this.menu_eventos);
            this.Controls.Add(this.menu_usuarios);
            this.Controls.Add(this.menu_acercade);
            this.Controls.Add(this.nomUsuario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.contenedor);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.encabezado);
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(1550, 900);
            this.Name = "Inicio";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Inicio_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.MenuStrip encabezado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel contenedor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label nomUsuario;
        private IconButton menu_acercade;
        private IconButton menu_ajustes;
        private IconButton menu_roles;
        private IconButton menu_usuarios;
        private IconButton menu_eventos;
        private IconButton btnexit;
    }
}

