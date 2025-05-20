using System;
using Datos.Models;

namespace Datos.Interfaces
{
    public interface ICredencialRepository : IRepository<Credencial>
    {
        Credencial GetByUsuario(string usuario);
        void RegistrarIntentoFallido(string legajo);
        void LimpiarIntentosFallidos(string legajo);
        void RegistrarBloqueo(string legajo);
        bool ValidarCredencial(string usuario, string password);
    }
} 