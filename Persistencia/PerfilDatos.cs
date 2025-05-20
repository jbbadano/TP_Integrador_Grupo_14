using System;
using System.Collections.Generic;
using System.Linq;
using Datos.Interfaces;
using Datos.Models;
using Persistencia.DataBase;

namespace Persistencia
{
    public class PerfilDatos
    {
        private readonly string _archivoPerfil = "perfil.csv";
        private readonly string _archivoPerfilRol = "perfil_rol.csv";
        private readonly DataBaseUtils _dataBaseUtils;

        public PerfilDatos()
        {
            _dataBaseUtils = new DataBaseUtils();
        }

        public Perfil ObtenerPorId(int idPerfil)
        {
            String descripcion = _dataBaseUtils.BuscarValor(_archivoPerfil, idPerfil.ToString(), 0, 1);
            if (string.IsNullOrEmpty(descripcion))
            {
                return null;
            }
            
            List<String> lstRolesData = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, idPerfil.ToString(), 0);
            List<Rol> roles = new List<Rol>();
            
            foreach (String rolData in lstRolesData)
            {
                String[] camposRol = rolData.Split(';');
                if (camposRol.Length >= 2)
                {
                    int idRol = Int32.Parse(camposRol[1]);
                    String descripcionRol = _dataBaseUtils.BuscarValor("rol.csv", idRol.ToString(), 0, 1);
                    
                    if (!string.IsNullOrEmpty(descripcionRol))
                    {
                        roles.Add(new Rol { IdRol = idRol, Codigo = descripcionRol, Descripcion = descripcionRol });
                    }
                }
            }
            
            return new Perfil { IdPerfil = idPerfil, Descripcion = descripcion, Roles = roles };
        }

        public List<Perfil> ObtenerTodos()
        {
            List<Perfil> lstPerfiles = new List<Perfil>();
            List<String> lstPerfilesData = _dataBaseUtils.ObtenerTodosLosRegistros(_archivoPerfil);
            
            foreach (String perfilData in lstPerfilesData)
            {
                String[] campos = perfilData.Split(';');
                if (campos.Length >= 2)
                {
                    int idPerfil = Int32.Parse(campos[0]);
                    String descripcion = campos[1];
                    
                    List<String> lstRolesData = _dataBaseUtils.BuscarRegistrosPorValor(_archivoPerfilRol, idPerfil.ToString(), 0);
                    List<Rol> roles = new List<Rol>();
                    
                    foreach (String rolData in lstRolesData)
                    {
                        String[] camposRol = rolData.Split(';');
                        if (camposRol.Length >= 2)
                        {
                            int idRol = Int32.Parse(camposRol[1]);
                            String descripcionRol = _dataBaseUtils.BuscarValor("rol.csv", idRol.ToString(), 0, 1);
                            
                            if (!string.IsNullOrEmpty(descripcionRol))
                            {
                                roles.Add(new Rol { IdRol = idRol, Codigo = descripcionRol, Descripcion = descripcionRol });
                            }
                        }
                    }
                    
                    lstPerfiles.Add(new Perfil { IdPerfil = idPerfil, Descripcion = descripcion, Roles = roles });
                }
            }
            
            return lstPerfiles;
        }
    }
} 