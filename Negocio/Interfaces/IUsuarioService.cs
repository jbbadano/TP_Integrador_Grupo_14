using System;
using System.Collections.Generic;
using Datos.Models;

namespace Negocio.Interfaces
{
    public interface IUsuarioService
    {
        Usuario ObtenerUsuario(string legajo);
        IEnumerable<Usuario> ObtenerTodos();
        void GuardarUsuario(Usuario usuario);
        void EliminarUsuario(string legajo);
    }
} 