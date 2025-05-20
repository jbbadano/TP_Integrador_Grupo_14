using System;
using System.Collections.Generic;
using System.Linq;
using Datos.Interfaces;
using Datos.Models;
using Persistencia.DataBase;

namespace Persistencia.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _archivoUsuarios = "usuarios.csv";
        private readonly string _archivoUsuarioPerfil = "usuario_perfil.csv";
        private readonly string _archivoPerfil = "perfil.csv";
        private readonly string _archivoPerfilRol = "perfil_rol.csv";
        private readonly string _archivoRoles = "rol.csv";
        private readonly DataBaseUtils _dataBaseUtils;

        public UsuarioRepository()
        {
            _dataBaseUtils = new DataBaseUtils();
        }

        public Usuario GetById(string id)
        {
            return GetByLegajo(id);
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            var registros = _dataBaseUtils.ObtenerTodosLosRegistros(_archivoUsuarios);
            
            foreach (var registro in registros)
            {
                var campos = registro.Split(';');
                if (campos.Length >= 4)
                {
                    var usuario = GetByLegajo(campos[0]);
                    if (usuario != null)
                    {
                        usuarios.Add(usuario);
                    }
                }
            }
            
            return usuarios;
        }

        public void Save(Usuario usuario)
        {
            _dataBaseUtils.ModificarRegistro(_archivoUsuarios, 0, usuario.Legajo, usuario.ToString());
        }

        public void Delete(string id)
        {
            _dataBaseUtils.EliminarRegistros(_archivoUsuarios, 0, id);
        }

        public Usuario GetByLegajo(string legajo)
        {
            var lstUsuarios = _dataBaseUtils.BuscarRegistrosPorValor(_archivoUsuarios, legajo, 0);
            if (lstUsuarios.Count == 0)
            {
                return null;
            }

            var campos = lstUsuarios[0].Split(';');
            if (campos.Length < 4)
            {
                return null;
            }

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

            return new Usuario
            {
                Legajo = campos[0],
                Nombre = campos[1],
                Apellido = campos[2],
                Email = campos[3],
                Perfil = new Perfil 
                { 
                    IdPerfil = int.Parse(idPerfil), 
                    Descripcion = descripcionPerfil, 
                    Roles = roles 
                }
            };
        }

        public Usuario GetByCredencial(Credencial credencial)
        {
            return GetByLegajo(credencial.Legajo);
        }
    }
} 