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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            errUsr.Visible = false;
            errClave.Visible = false;
            errMsj.Visible = false;

            String usuario = txtUsuario.Text;
            String password = txtClave.Text;

            if (usuario.Equals("") || password.Equals(""))                                      // Validar datos obligatorios
            {
                if (string.IsNullOrEmpty(usuario))
                {
                    errUsr.Visible = true;                                                      // Marcar error en el campo de usuario
                }

                if (string.IsNullOrEmpty(password))
                {
                    errClave.Visible = true;                                                    // Marcar error en el campo de clave
                }
                errMsj.Text = "* Completar datos obligatorios";
                errMsj.Visible = true;
            }
            else
            {
                LoginNegocio loginNegocio = new LoginNegocio();                                 
                Credencial credencial = loginNegocio.Login(usuario, password);

                if (credencial == null)                                                         // Si loginNegocio.login devuelve null: usuario o clave son incorrectos
                {
                    errUsr.Visible = true;                                                      // Marcar error en el campo de usuario
                    errClave.Visible = true;                                                    // Marcar error en el campo de clave actual
                    errMsj.Text = "* Usuario o clave incorrectos";                              // Mostrar mensaje de error
                    errMsj.Visible = true;
                }
                else if(credencial.Bloqueado)                                                     // Si devuelve una credencial verificar que no esté bloqueada
                {
                    errMsj.Text = "* Usuario bloqueado, \r\n contactese con el administrador";       // Mostrar mensaje de error
                    errMsj.Visible = true;
                }
                else if (credencial.PrimerIngreso)                                              // Si no está bloqueada verificar si es el primer ingreso
                {
                    MessageBox.Show("Primer ingreso debe registrar una clave");

                    this.Hide();
                    FormCambioClave formCambioClave = new FormCambioClave(credencial);
                    formCambioClave.ShowDialog();
                    this.Close();
                }
                else if(credencial.ClaveVencida())                                              // Si no está bloqueada verificar que no esté vencida la clave
                {
                    MessageBox.Show("Clave vencida se requiere cambio de clave");

                    this.Hide();
                    FormCambioClave formCambioClave = new FormCambioClave(credencial);
                    formCambioClave.ShowDialog();
                    this.Close();
                }
                else
                {
                    this.Hide();
                    FormMenu formMenu= new FormMenu(credencial);
                    formMenu.ShowDialog();
                    this.Close();
                }
            }
        }

        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCambioClave formCambioClave = new FormCambioClave();
            formCambioClave.ShowDialog();
            this.Close();
        }
    }
}
