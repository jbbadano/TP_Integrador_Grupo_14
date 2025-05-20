using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Models;
using Persistencia;

namespace Negocio
{
    public class LoginNegocio
    {
        private readonly CredencialDatos _credencialDatos;
        private readonly PerfilDatos _perfilDatos;
        private readonly RolDatos _rolDatos;

        public LoginNegocio()
        {
            _credencialDatos = new CredencialDatos();
            _perfilDatos = new PerfilDatos();
            _rolDatos = new RolDatos();
        }

        public Credencial Login(string usuario, string password)
        {
            Console.WriteLine($"Intentando login para usuario: {usuario}");
            
            // Obtener credencial
            Credencial credencial = _credencialDatos.ObtenerPorUsuario(usuario);
            if (credencial == null)
            {
                Console.WriteLine("Credencial no encontrada");
                return null;
            }

            Console.WriteLine($"Credencial encontrada. Contraseña almacenada: {credencial.PasswordHash}");
            Console.WriteLine($"Contraseña ingresada: {password}");

            // Verificar contraseña
            if (credencial.PasswordHash != password)
            {
                Console.WriteLine("Contraseña incorrecta");
                return null;
            }

            // Obtener perfil y roles
            Perfil perfil = _perfilDatos.ObtenerPorId(credencial.IdPerfil);
            if (perfil != null)
            {
                credencial.DescripcionPerfil = perfil.Descripcion;
                credencial.Roles = perfil.Roles;
                Console.WriteLine($"Perfil encontrado: {perfil.Descripcion}");
            }

            // Actualizar último login
            credencial.FechaUltimoLogin = DateTime.Now;
            _credencialDatos.Actualizar(credencial);

            return credencial;
        }

        public void CambiarClave(Credencial credencial_con_nueva_clave)
        {
            credencial_con_nueva_clave.FechaCambioClave = DateTime.Now;
            credencial_con_nueva_clave.PrimerIngreso = false;
            _credencialDatos.Actualizar(credencial_con_nueva_clave);
        }
    }
}
