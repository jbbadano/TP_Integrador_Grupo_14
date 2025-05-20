using System;
using Datos.Models;

namespace Negocio.Interfaces
{
    public class ResultadoAutenticacion
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public Usuario Usuario { get; set; }
    }

    public interface IAutenticacionService
    {
        ResultadoAutenticacion Autenticar(string usuario, string password);
        void RegistrarIntentoFallido(string legajo);
        void LimpiarIntentosFallidos(string legajo);
        void BloquearUsuario(string legajo);
    }
} 