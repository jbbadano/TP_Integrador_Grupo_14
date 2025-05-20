using System;
using System.Collections.Generic;

namespace Datos.Models
{
    public class Credencial
    {
        public string Legajo { get; set; }
        public string Usuario { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Estado { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
        public int IdPerfil { get; set; }
        public string DescripcionPerfil { get; set; }
        public List<Rol> Roles { get; set; }
        public DateTime FechaUltimoLogin { get; set; }
        public DateTime? FechaCambioClave { get; set; }
        public bool PrimerIngreso { get; set; }

        public Credencial()
        {
            Roles = new List<Rol>();
        }

        public Credencial(string strCredencial, int intentos, bool bloqueo, string descripcionPerfil, List<Rol> roles)
        {
            string[] campos = strCredencial.Split(';');
            if (campos.Length >= 5)
            {
                Legajo = campos[0];
                Usuario = campos[1];
                PasswordHash = campos[2];
                FechaAlta = DateTime.Parse(campos[3]);
                FechaBaja = string.IsNullOrEmpty(campos[4]) ? (DateTime?)null : DateTime.Parse(campos[4]);
                Estado = campos.Length > 5 ? campos[5] : "";
            }
            
            IntentosFallidos = intentos;
            Bloqueado = bloqueo;
            DescripcionPerfil = descripcionPerfil;
            Roles = roles != null ? roles : new List<Rol>();
        }

        public string ToStringCSV()
        {
            string fechaBajaStr = FechaBaja.HasValue ? FechaBaja.Value.ToString("dd/MM/yyyy") : "";
            return $"{Legajo};{Usuario};{PasswordHash};{FechaAlta:dd/MM/yyyy};{fechaBajaStr};{Estado}";
        }

        public string Bienvenida()
        {
            return $"Bienvenido/a {Usuario}";
        }

        public bool TieneRol(string codigoRol)
        {
            return Roles.Exists(r => r.Codigo.Equals(codigoRol, StringComparison.OrdinalIgnoreCase));
        }

        public bool ClaveVencida()
        {
            if (!FechaCambioClave.HasValue)
                return true;

            return (DateTime.Now - FechaCambioClave.Value).TotalDays > 30;
        }
    }
} 