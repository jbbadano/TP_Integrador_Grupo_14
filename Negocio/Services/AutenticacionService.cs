using System;
using Datos.Interfaces;
using Negocio.Interfaces;

namespace Negocio.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly ICredencialRepository _credencialRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private const int MAX_INTENTOS = 3;

        public AutenticacionService(
            ICredencialRepository credencialRepository,
            IUsuarioRepository usuarioRepository)
        {
            _credencialRepository = credencialRepository;
            _usuarioRepository = usuarioRepository;
        }

        public ResultadoAutenticacion Autenticar(string usuario, string password)
        {
            var credencial = _credencialRepository.GetByUsuario(usuario);
            if (credencial == null)
            {
                return new ResultadoAutenticacion
                {
                    Exitoso = false,
                    Mensaje = "Usuario no encontrado"
                };
            }

            if (credencial.Bloqueado)
            {
                return new ResultadoAutenticacion
                {
                    Exitoso = false,
                    Mensaje = "Usuario bloqueado"
                };
            }

            if (!credencial.PasswordHash.Equals(password))
            {
                RegistrarIntentoFallido(credencial.Legajo);
                
                if (credencial.IntentosFallidos + 1 >= MAX_INTENTOS)
                {
                    BloquearUsuario(credencial.Legajo);
                    return new ResultadoAutenticacion
                    {
                        Exitoso = false,
                        Mensaje = "Usuario bloqueado por exceder intentos fallidos"
                    };
                }

                return new ResultadoAutenticacion
                {
                    Exitoso = false,
                    Mensaje = "Contraseña incorrecta"
                };
            }

            LimpiarIntentosFallidos(credencial.Legajo);
            
            var usuarioCompleto = _usuarioRepository.GetByCredencial(credencial);
            return new ResultadoAutenticacion
            {
                Exitoso = true,
                Mensaje = "Autenticación exitosa",
                Usuario = usuarioCompleto
            };
        }

        public void RegistrarIntentoFallido(string legajo)
        {
            _credencialRepository.RegistrarIntentoFallido(legajo);
        }

        public void LimpiarIntentosFallidos(string legajo)
        {
            _credencialRepository.LimpiarIntentosFallidos(legajo);
        }

        public void BloquearUsuario(string legajo)
        {
            _credencialRepository.RegistrarBloqueo(legajo);
        }
    }
} 