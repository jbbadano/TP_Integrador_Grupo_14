namespace TP_Integrador
{
    partial class FormMenu
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
            this.lblNombreUsuario = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnOperador = new System.Windows.Forms.Button();
            this.btnModificarPersona = new System.Windows.Forms.Button();
            this.btnAutorizarModificarPersona = new System.Windows.Forms.Button();
            this.btnDesbloquearCredencial = new System.Windows.Forms.Button();
            this.btnAutorizarDesbloquearCredencial = new System.Windows.Forms.Button();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNombreUsuario
            // 
            this.lblNombreUsuario.AutoSize = true;
            this.lblNombreUsuario.Location = new System.Drawing.Point(12, 9);
            this.lblNombreUsuario.Name = "lblNombreUsuario";
            this.lblNombreUsuario.Size = new System.Drawing.Size(35, 13);
            this.lblNombreUsuario.TabIndex = 0;
            this.lblNombreUsuario.Text = "label1";
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(116, 182);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(136, 23);
            this.btnSalir.TabIndex = 48;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnOperador
            // 
            this.btnOperador.Location = new System.Drawing.Point(57, 58);
            this.btnOperador.Name = "btnOperador";
            this.btnOperador.Size = new System.Drawing.Size(136, 23);
            this.btnOperador.TabIndex = 49;
            this.btnOperador.Text = "Operador";
            this.btnOperador.UseVisualStyleBackColor = true;
            this.btnOperador.Click += new System.EventHandler(this.btnOperador_Click);
            // 
            // btnModificarPersona
            // 
            this.btnModificarPersona.Location = new System.Drawing.Point(57, 87);
            this.btnModificarPersona.Name = "btnModificarPersona";
            this.btnModificarPersona.Size = new System.Drawing.Size(136, 23);
            this.btnModificarPersona.TabIndex = 50;
            this.btnModificarPersona.Text = "Modificar Persona";
            this.btnModificarPersona.UseVisualStyleBackColor = true;
            // 
            // btnAutorizarModificarPersona
            // 
            this.btnAutorizarModificarPersona.Location = new System.Drawing.Point(199, 87);
            this.btnAutorizarModificarPersona.Name = "btnAutorizarModificarPersona";
            this.btnAutorizarModificarPersona.Size = new System.Drawing.Size(136, 23);
            this.btnAutorizarModificarPersona.TabIndex = 51;
            this.btnAutorizarModificarPersona.Text = "Autorizar";
            this.btnAutorizarModificarPersona.UseVisualStyleBackColor = true;
            // 
            // btnDesbloquearCredencial
            // 
            this.btnDesbloquearCredencial.Location = new System.Drawing.Point(57, 116);
            this.btnDesbloquearCredencial.Name = "btnDesbloquearCredencial";
            this.btnDesbloquearCredencial.Size = new System.Drawing.Size(136, 23);
            this.btnDesbloquearCredencial.TabIndex = 52;
            this.btnDesbloquearCredencial.Text = "Desbloquear Credencial";
            this.btnDesbloquearCredencial.UseVisualStyleBackColor = true;
            // 
            // btnAutorizarDesbloquearCredencial
            // 
            this.btnAutorizarDesbloquearCredencial.Location = new System.Drawing.Point(199, 116);
            this.btnAutorizarDesbloquearCredencial.Name = "btnAutorizarDesbloquearCredencial";
            this.btnAutorizarDesbloquearCredencial.Size = new System.Drawing.Size(136, 23);
            this.btnAutorizarDesbloquearCredencial.TabIndex = 53;
            this.btnAutorizarDesbloquearCredencial.Text = "Autorizar";
            this.btnAutorizarDesbloquearCredencial.UseVisualStyleBackColor = true;
            // 
            // lblPerfil
            // 
            this.lblPerfil.Location = new System.Drawing.Point(196, 9);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(180, 13);
            this.lblPerfil.TabIndex = 54;
            this.lblPerfil.Text = "label1";
            this.lblPerfil.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 236);
            this.Controls.Add(this.lblPerfil);
            this.Controls.Add(this.btnAutorizarDesbloquearCredencial);
            this.Controls.Add(this.btnDesbloquearCredencial);
            this.Controls.Add(this.btnAutorizarModificarPersona);
            this.Controls.Add(this.btnModificarPersona);
            this.Controls.Add(this.btnOperador);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblNombreUsuario);
            this.Name = "FormMenu";
            this.Text = "Menú";
            this.Load += new System.EventHandler(this.FormMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNombreUsuario;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnOperador;
        private System.Windows.Forms.Button btnModificarPersona;
        private System.Windows.Forms.Button btnAutorizarModificarPersona;
        private System.Windows.Forms.Button btnDesbloquearCredencial;
        private System.Windows.Forms.Button btnAutorizarDesbloquearCredencial;
        private System.Windows.Forms.Label lblPerfil;
    }
}