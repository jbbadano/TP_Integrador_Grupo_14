using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos.Models;
using Negocio;

namespace TP_Integrador
{
    public partial class FormMenu : Form
    {
        private Credencial _credencial;

        public FormMenu(Credencial credencial)
        {
            InitializeComponent();
            _credencial = credencial;
            lblNombreUsuario.Text = credencial.Bienvenida();
            lblPerfil.Text = credencial.DescripcionPerfil;

            ConfigurarMenuSegunPerfil();
        }

        private void ConfigurarMenuSegunPerfil()
        {
            // Deshabilitar todos los botones por defecto
            btnModificarPersona.Enabled = false;
            btnDesbloquearCredencial.Enabled = false;
            btnAutorizarModificarPersona.Enabled = false;
            btnAutorizarDesbloquearCredencial.Enabled = false;
            btnOperador.Enabled = false;

            // Configurar según el perfil
            switch (_credencial.IdPerfil)
            {
                case 1: // Supervisor
                    btnModificarPersona.Enabled = _credencial.TieneRol("MOD_PERSONA");
                    btnDesbloquearCredencial.Enabled = _credencial.TieneRol("DESB_CRED");
                    break;

                case 2: // Administrador
                    btnAutorizarModificarPersona.Enabled = true;
                    btnAutorizarDesbloquearCredencial.Enabled = true;
                    break;

                case 3: // Operador
                    btnOperador.Enabled = true;
                    break;
            }
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
            this.Close();
        }

        private void btnModificarPersona_Click(object sender, EventArgs e)
        {
            // TODO: Implementar formulario de modificación de persona
            MessageBox.Show("Funcionalidad en desarrollo");
        }

        private void btnDesbloquearCredencial_Click(object sender, EventArgs e)
        {
            // TODO: Implementar formulario de desbloqueo de credencial
            MessageBox.Show("Funcionalidad en desarrollo");
        }

        private void btnAutorizarModificarPersona_Click(object sender, EventArgs e)
        {
            // TODO: Implementar formulario de autorizaciones de modificación
            MessageBox.Show("Funcionalidad en desarrollo");
        }

        private void btnAutorizarDesbloquearCredencial_Click(object sender, EventArgs e)
        {
            // TODO: Implementar formulario de autorizaciones de desbloqueo
            MessageBox.Show("Funcionalidad en desarrollo");
        }

        private void btnOperador_Click(object sender, EventArgs e)
        {
            // TODO: Implementar funcionalidad de operador (Fase 2)
            MessageBox.Show("Funcionalidad disponible en la Fase 2");
        }
    }
}
