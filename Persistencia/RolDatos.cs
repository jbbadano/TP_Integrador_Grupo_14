using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos.Interfaces;
using Datos.Models;
using Persistencia.DataBase;

namespace Persistencia
{
    public class RolDatos
    {
        private readonly string _archivoRoles = "rol.csv";

        public List<Rol> ObtenerTodos()
        {
            List<Rol> roles = new List<Rol>();
            if (!File.Exists(_archivoRoles))
                return roles;

            string[] lineas = File.ReadAllLines(_archivoRoles);
            foreach (string linea in lineas)
            {
                string[] datos = linea.Split(';');
                Rol rol = new Rol
                {
                    IdRol = int.Parse(datos[0]),
                    Codigo = datos[1],
                    Descripcion = datos[2]
                };
                roles.Add(rol);
            }
            return roles;
        }
    }
} 