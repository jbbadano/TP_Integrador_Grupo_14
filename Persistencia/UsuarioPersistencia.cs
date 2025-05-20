using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Persistencia.DataBase;
using Datos.Interfaces;
using Datos.Models;

namespace Persistencia
{
    public class UsuarioPersistencia
    {
        private readonly string _archivoUsuarios = "usuarios.csv";
        private readonly string _archivoUsuarioPerfil = "usuario_perfil.csv";
        private readonly string _archivoPerfil = "perfil.csv";
        private readonly string _archivoPerfilRol = "perfil_rol.csv";
        private readonly string _archivoRoles = "rol.csv";
        private readonly DataBaseUtils _dataBaseUtils;

        public UsuarioPersistencia()
        {
            _dataBaseUtils = new DataBaseUtils();
        }

        public Credencial Login(String nombreUsuario)
        {
            Credencial salida = null;

            List<String> lstCredenciales = _dataBaseUtils.BuscarRegistrosPorValor("credenciales.csv", nombreUsuario, 1);
            if (lstCredenciales.Count() > 0)
            {
                String strCredencial = lstCredenciales[0];
                String[] camposCredencial = strCredencial.Split(';');

                List<String> intentosFallidosUser = _dataBaseUtils.BuscarRegistrosPorValor("login_intentos.csv", camposCredencial[0], 0);
                int intentos = intentosFallidosUser.Count();

                List<String> bloqueosUser = _dataBaseUtils.BuscarRegistrosPorValor("usuario_bloqueado.csv", camposCredencial[0], 0);
                Boolean bloqueo = bloqueosUser.Count() > 0;

                String strUsuarioPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, camposCredencial[0], 0, 1);
                int usuarioPerfil = 0;
                if (int.TryParse(strUsuarioPerfil, out int result))
                {
                    usuarioPerfil = result;
                }
                String descripcionPerfil = _dataBaseUtils.BuscarValor(_archivoPerfil, usuarioPerfil.ToString(), 0, 1);

                List<Rol> roles = new List<Rol>();
                List<String> rolesData = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, usuarioPerfil.ToString(), 0);
                foreach (String rolData in rolesData)
                {
                    String[] camposRol = rolData.Split(';');
                    int idRol = Int32.Parse(camposRol[1]);
                    String codigoRol = camposRol[2];
                    String denRol = _dataBaseUtils.BuscarValor(_archivoRoles, idRol.ToString(), 0, 1);
                    roles.Add(new Rol { IdRol = idRol, Codigo = codigoRol, Descripcion = denRol });
                }
                salida = new Credencial(strCredencial, intentos, bloqueo, descripcionPerfil, roles);
            }
            return salida;
        }

        public void ActualizarCredencial(Credencial credencial)
        {
            _dataBaseUtils.ModificarRegistro("credenciales.csv", 1, credencial.Usuario, credencial.ToStringCSV());
        }

        public void RegistrarIntentoFallido(Credencial credencial)
        {
            String registro = credencial.Legajo + ";" + DateTime.Now.ToString("dd/MM/yyyy");
            _dataBaseUtils.AgregarRegistro("login_intentos.csv", registro);
        }

        public void LimpiarIntentosFallidos(Credencial credencial)
        {
            _dataBaseUtils.EliminarRegistros("login_intentos.csv", 0, credencial.Legajo);
        }

        public void RegistrarBloqueo(Credencial credencial)
        {
            _dataBaseUtils.AgregarRegistro("usuario_bloqueado.csv", credencial.Legajo);
        }

        public Usuario ObtenerPorLegajo(string legajo)
        {
            List<String> lstUsuarios = _dataBaseUtils.BuscarRegistrosPorValor(_archivoUsuarios, legajo, 0);
            if (lstUsuarios.Count == 0)
            {
                return null;
            }

            String[] campos = lstUsuarios[0].Split(';');
            if (campos.Length < 4)
            {
                return null;
            }

            String nombre = campos[1];
            String apellido = campos[2];
            String email = campos[3];

            String idPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, legajo, 0, 1);
            if (string.IsNullOrEmpty(idPerfil))
            {
                return null;
            }

            String descripcionPerfil = _dataBaseUtils.BuscarValor(_archivoPerfil, idPerfil, 0, 1);
            if (string.IsNullOrEmpty(descripcionPerfil))
            {
                return null;
            }

            List<String> lstRoles = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, idPerfil, 0);
            List<Rol> roles = new List<Rol>();

            foreach (String rolData in lstRoles)
            {
                String[] camposRol = rolData.Split(';');
                if (camposRol.Length >= 2)
                {
                    int idRol = Int32.Parse(camposRol[1]);
                    String descripcionRol = _dataBaseUtils.BuscarValor(_archivoRoles, idRol.ToString(), 0, 1);
                    
                    if (!string.IsNullOrEmpty(descripcionRol))
                    {
                        roles.Add(new Rol { IdRol = idRol, Codigo = descripcionRol, Descripcion = descripcionRol });
                    }
                }
            }

            return new Usuario
            {
                Legajo = legajo,
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Perfil = new Perfil { IdPerfil = int.Parse(idPerfil), Descripcion = descripcionPerfil, Roles = roles }
            };
        }

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lstUsuarios = new List<Usuario>();
            List<String> lstUsuariosData = _dataBaseUtils.ObtenerTodosLosRegistros(_archivoUsuarios);

            foreach (String usuarioData in lstUsuariosData)
            {
                String[] campos = usuarioData.Split(';');
                if (campos.Length < 4)
                {
                    continue;
                }

                String legajo = campos[0];
                String nombre = campos[1];
                String apellido = campos[2];
                String email = campos[3];

                String idPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, legajo, 0, 1);
                if (string.IsNullOrEmpty(idPerfil))
                {
                    continue;
                }

                String descripcionPerfil = _dataBaseUtils.BuscarValor(_archivoPerfil, idPerfil, 0, 1);
                if (string.IsNullOrEmpty(descripcionPerfil))
                {
                    continue;
                }

                List<String> lstRoles = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, idPerfil, 0);
                List<Rol> roles = new List<Rol>();

                foreach (String rolData in lstRoles)
                {
                    String[] camposRol = rolData.Split(';');
                    if (camposRol.Length >= 2)
                    {
                        int idRol = Int32.Parse(camposRol[1]);
                        String descripcionRol = _dataBaseUtils.BuscarValor(_archivoRoles, idRol.ToString(), 0, 1);
                        
                        if (!string.IsNullOrEmpty(descripcionRol))
                        {
                            roles.Add(new Rol { IdRol = idRol, Codigo = descripcionRol, Descripcion = descripcionRol });
                        }
                    }
                }

                lstUsuarios.Add(new Usuario
                {
                    Legajo = legajo,
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Perfil = new Perfil { IdPerfil = int.Parse(idPerfil), Descripcion = descripcionPerfil, Roles = roles }
                });
            }

            return lstUsuarios;
        }
    }
}
