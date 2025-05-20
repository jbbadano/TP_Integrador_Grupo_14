using System;
using System.Collections.Generic;
using Datos.Interfaces;
using Datos.Models;
using Negocio.Interfaces;

namespace Negocio.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICredencialRepository _credencialRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ICredencialRepository credencialRepository)
        {
            _usuarioRepository = usuarioRepository;
            _credencialRepository = credencialRepository;
        }

        public Usuario ObtenerUsuario(string legajo)
        {
            return _usuarioRepository.GetByLegajo(legajo);
        }

        public IEnumerable<Usuario> ObtenerTodos()
        {
            return _usuarioRepository.GetAll();
        }

        public void GuardarUsuario(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Legajo) || 
                string.IsNullOrEmpty(usuario.Nombre) || 
                string.IsNullOrEmpty(usuario.Apellido) || 
                string.IsNullOrEmpty(usuario.Email))
            {
                throw new ArgumentException("Todos los campos son obligatorios");
            }

            _usuarioRepository.Save(usuario);
        }

        public void EliminarUsuario(string legajo)
        {
            var usuario = _usuarioRepository.GetByLegajo(legajo);
            if (usuario == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }

            _usuarioRepository.Delete(legajo);
        }
    }
} 