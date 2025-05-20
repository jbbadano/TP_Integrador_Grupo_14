using System;
using System.Collections.Generic;
using Datos.Models;

namespace Datos.Interfaces
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        Perfil GetById(int id);
        new IEnumerable<Perfil> GetAll();
    }
} 