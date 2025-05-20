using System;
using Datos.Models;

namespace Datos.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario GetByLegajo(string legajo);
        Usuario GetByCredencial(Credencial credencial);
    }
} 