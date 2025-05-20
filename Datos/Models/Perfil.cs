using System;
using System.Collections.Generic;

namespace Datos.Models
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string Descripcion { get; set; }
        public List<Rol> Roles { get; set; }

        public Perfil()
        {
            Roles = new List<Rol>();
        }

        public override string ToString()
        {
            return $"{IdPerfil};{Descripcion}";
        }
    }
} 