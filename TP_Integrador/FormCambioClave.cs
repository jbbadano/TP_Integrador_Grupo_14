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
    public partial class FormCambioClave : Form
    {
        private Credencial _credencial;

        public FormCambioClave()
        {
            InitializeComponent();
        }

        public FormCambioClave(Credencial credencial)
        {
            InitializeComponent();
            _credencial = credencial;
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            errClaveActual.Visible = false;
            errClaveNva1.Visible = false;
            errClaveNva2.Visible = false;
            errMsj.Visible = false;

            String claveActual = txtClaveActual.Text;
            String claveNueva = txtClaveNva1.Text;
            String claveNueva2 = txtClaveNva2.Text;

            if (claveActual.Equals("") || claveNueva.Equals("") || claveNueva2.Equals(""))
            {
                if (string.IsNullOrEmpty(claveActual))
                {
                    errClaveActual.Visible = true;
                }

                if (string.IsNullOrEmpty(claveNueva))
                {
                    errClaveNva1.Visible = true;
                }

                if (string.IsNullOrEmpty(claveNueva2))
                {
                    errClaveNva2.Visible = true;
                }

                errMsj.Text = "* Completar datos obligatorios";
                errMsj.Visible = true;
            }
            else
            {
                if (!claveNueva.Equals(claveNueva2))
                {
                    errClaveNva1.Visible = true;
                    errClaveNva2.Visible = true;
                    errMsj.Text = "* Las claves nuevas no coinciden";
                    errMsj.Visible = true;
                }
                else
                {
                    LoginNegocio loginNegocio = new LoginNegocio();
                    Credencial credencial = loginNegocio.Login(_credencial.Usuario, claveActual);

                    if (credencial == null)
                    {
                        errClaveActual.Visible = true;
                        errMsj.Text = "* Clave actual incorrecta";
                        errMsj.Visible = true;
                    }
                    else
                    {
                        credencial.PasswordHash = claveNueva;
                        loginNegocio.CambiarClave(credencial);

                        MessageBox.Show("Clave cambiada exitosamente");

                        this.Hide();
                        FormMenu formMenu = new FormMenu(credencial);
                        formMenu.ShowDialog();
                        this.Close();
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            btnCambiar_Click(sender, e);
        }
    }
}
