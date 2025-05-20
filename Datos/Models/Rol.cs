using System;

namespace Datos.Models
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return $"{IdRol};{Codigo};{Descripcion}";
        }
    }
} 