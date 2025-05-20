using System;
using System.Collections.Generic;

namespace Datos.Models
{
    public class Usuario
    {
        public string Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public Perfil Perfil { get; set; }
        public Credencial Credencial { get; set; }

        public override string ToString()
        {
            return $"{Legajo};{Nombre};{Apellido};{Email}";
        }
    }
} 