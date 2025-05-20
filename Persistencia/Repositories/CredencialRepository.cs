using System;
using System.Collections.Generic;
using System.Linq;
using Datos.Interfaces;
using Datos.Models;
using Persistencia.DataBase;

namespace Persistencia.Repositories
{
    public class CredencialRepository : ICredencialRepository
    {
        private readonly string _archivoCredenciales = "credenciales.csv";
        private readonly string _archivoUsuarioPerfil = "usuario_perfil.csv";
        private readonly string _archivoPerfil = "perfil.csv";
        private readonly string _archivoPerfilRol = "perfil_rol.csv";
        private readonly string _archivoRoles = "rol.csv";
        private readonly DataBaseUtils _dataBaseUtils;

        public CredencialRepository()
        {
            _dataBaseUtils = new DataBaseUtils();
        }

        public Credencial GetById(string id)
        {
            return GetByUsuario(id);
        }

        public IEnumerable<Credencial> GetAll()
        {
            var credenciales = new List<Credencial>();
            var registros = _dataBaseUtils.ObtenerTodosLosRegistros(_archivoCredenciales);
            
            foreach (var registro in registros)
            {
                var credencial = GetByUsuario(registro.Split(';')[1]);
                if (credencial != null)
                {
                    credenciales.Add(credencial);
                }
            }
            
            return credenciales;
        }

        public void Save(Credencial credencial)
        {
            _dataBaseUtils.ModificarRegistro(_archivoCredenciales, 1, credencial.Usuario, credencial.ToStringCSV());
        }

        public void Delete(string id)
        {
            _dataBaseUtils.EliminarRegistros(_archivoCredenciales, 1, id);
        }

        public Credencial GetByUsuario(string usuario)
        {
            var lstCredenciales = _dataBaseUtils.BuscarRegistrosPorValor(_archivoCredenciales, usuario, 1);
            if (lstCredenciales.Count == 0)
            {
                return null;
            }

            var strCredencial = lstCredenciales[0];
            var camposCredencial = strCredencial.Split(';');
            
            if (camposCredencial.Length < 5)
            {
                return null;
            }

            var legajo = camposCredencial[0];
            var intentosFallidos = _dataBaseUtils.BuscarRegistrosPorValor("login_intentos.csv", legajo, 0).Count;
            var bloqueado = _dataBaseUtils.BuscarRegistrosPorValor("usuario_bloqueado.csv", legajo, 0).Count > 0;

            var idPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, legajo, 0, 1);
            if (string.IsNullOrEmpty(idPerfil))
            {
                return null;
            }

            var descripcionPerfil = _dataBaseUtils.BuscarValor(_archivoPerfil, idPerfil, 0, 1);
            if (string.IsNullOrEmpty(descripcionPerfil))
            {
                return null;
            }

            var roles = new List<Rol>();
            var rolesData = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, idPerfil, 0);
            
            foreach (var rolData in rolesData)
            {
                var camposRol = rolData.Split(';');
                if (camposRol.Length >= 2)
                {
                    var idRol = int.Parse(camposRol[1]);
                    var descripcionRol = _dataBaseUtils.BuscarValor(_archivoRoles, idRol.ToString(), 0, 1);
                    
                    if (!string.IsNullOrEmpty(descripcionRol))
                    {
                        roles.Add(new Rol { IdRol = idRol, Codigo = descripcionRol, Descripcion = descripcionRol });
                    }
                }
            }

            return new Credencial(strCredencial, intentosFallidos, bloqueado, descripcionPerfil, roles);
        }

        public void RegistrarIntentoFallido(string legajo)
        {
            var registro = $"{legajo};{DateTime.Now:dd/MM/yyyy}";
            _dataBaseUtils.AgregarRegistro("login_intentos.csv", registro);
        }

        public void LimpiarIntentosFallidos(string legajo)
        {
            _dataBaseUtils.EliminarRegistros("login_intentos.csv", 0, legajo);
        }

        public void RegistrarBloqueo(string legajo)
        {
            _dataBaseUtils.AgregarRegistro("usuario_bloqueado.csv", legajo);
        }

        public bool ValidarCredencial(string usuario, string password)
        {
            var credencial = GetByUsuario(usuario);
            if (credencial == null)
            {
                return false;
            }

            return credencial.PasswordHash.Equals(password);
        }
    }
} 