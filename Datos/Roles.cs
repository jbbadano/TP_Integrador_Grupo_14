using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Roles
    {
        //Atributos
        private int _idRol;
        private String _descripcionRol;

        //Propiedades
        public int IdRol { get => _idRol; set => _idRol = value; }
        public String DescripcionRol { get => _descripcionRol; set => _descripcionRol = value; }
        //Constructor
        public Roles(int idRol, String nombreRol)
        {
            this._idRol = idRol;
            this._descripcionRol = nombreRol;
        }
    }
}
