using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos.Interfaces;
using Datos.Models;
using Persistencia.DataBase;

namespace Persistencia
{
    public class CredencialDatos
    {
        private readonly string _archivoCredenciales = "credenciales.csv";
        private readonly string _archivoUsuarioPerfil = "usuario_perfil.csv";
        private readonly DataBaseUtils _dataBaseUtils;

        public CredencialDatos()
        {
            Console.WriteLine("=== INICIO CredencialDatos ===");
            _dataBaseUtils = new DataBaseUtils();
            Console.WriteLine("=== FIN CredencialDatos ===");
        }

        public Credencial ObtenerPorUsuario(string usuario)
        {
            try
            {
                Console.WriteLine($"\n=== INICIO ObtenerPorUsuario ===");
                Console.WriteLine($"Usuario a buscar: {usuario}");

                Console.WriteLine($"\n1. Buscando credenciales para usuario {usuario}");
                List<String> lstCredenciales = _dataBaseUtils.BuscarRegistrosPorValor(_archivoCredenciales, usuario, 1);
                Console.WriteLine($"Resultados encontrados: {lstCredenciales.Count}");
                
                if (lstCredenciales.Count == 0)
                {
                    Console.WriteLine("ERROR: No se encontraron credenciales para el usuario");
                    return null;
                }

                String strCredencial = lstCredenciales[0];
                Console.WriteLine($"\n2. Procesando credencial encontrada: {strCredencial}");
                String[] camposCredencial = strCredencial.Split(';');
                Console.WriteLine($"Número de campos en credencial: {camposCredencial.Length}");
                Console.WriteLine($"Campos: [{string.Join(" | ", camposCredencial)}]");

                if (camposCredencial.Length < 5)
                {
                    Console.WriteLine("ERROR: La credencial no tiene suficientes campos");
                    return null;
                }

                string legajo = camposCredencial[0];
                string nombreUsuario = camposCredencial[1];
                string password = camposCredencial[2];
                string fechaAlta = camposCredencial[3];
                string fechaBaja = camposCredencial[4];
                string estado = camposCredencial.Length > 5 ? camposCredencial[5] : "";

                Console.WriteLine($"\n3. Datos de credencial:");
                Console.WriteLine($"- Legajo: {legajo}");
                Console.WriteLine($"- Usuario: {nombreUsuario}");
                Console.WriteLine($"- Password: {password}");
                Console.WriteLine($"- Fecha Alta: {fechaAlta}");
                Console.WriteLine($"- Fecha Baja: {fechaBaja}");
                Console.WriteLine($"- Estado: {estado}");

                // Intentos fallidos
                int intentos = 0;
                try
                {
                    Console.WriteLine($"\n4. Buscando intentos fallidos para legajo {legajo}");
                    List<String> intentosFallidosUser = _dataBaseUtils.BuscarRegistrosPorValor("login_intentos.csv", legajo, 0);
                    Console.WriteLine($"Intentos fallidos encontrados: {intentosFallidosUser.Count}");
                    intentos = intentosFallidosUser.Count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR al buscar intentos fallidos: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    intentos = 0;
                }

                // Bloqueo
                Boolean bloqueo = false;
                try
                {
                    Console.WriteLine($"\n5. Verificando bloqueo para legajo {legajo}");
                    List<String> bloqueosUser = _dataBaseUtils.BuscarRegistrosPorValor("usuario_bloqueado.csv", legajo, 0);
                    Console.WriteLine($"Bloqueos encontrados: {bloqueosUser.Count}");
                    bloqueo = bloqueosUser.Count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR al verificar bloqueo: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    bloqueo = false;
                }

                Console.WriteLine($"\n6. Buscando perfil para legajo {legajo}");
                String strUsuarioPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, legajo, 0, 1);
                Console.WriteLine($"Perfil encontrado: {strUsuarioPerfil}");
                
                if (string.IsNullOrEmpty(strUsuarioPerfil))
                {
                    Console.WriteLine("ERROR: No se encontró el perfil para el usuario");
                    return null;
                }

                int usuarioPerfil = 0;
                if (int.TryParse(strUsuarioPerfil, out int result))
                {
                    usuarioPerfil = result;
                    Console.WriteLine($"ID de perfil parseado: {usuarioPerfil}");
                }
                else
                {
                    Console.WriteLine("ERROR: No se pudo parsear el ID de perfil");
                    return null;
                }

                Console.WriteLine($"\n7. Buscando descripción del perfil {usuarioPerfil}");
                String descripcionPerfil = _dataBaseUtils.BuscarValor("perfil.csv", usuarioPerfil.ToString(), 0, 1);
                Console.WriteLine($"Descripción de perfil encontrada: {descripcionPerfil}");

                if (string.IsNullOrEmpty(descripcionPerfil))
                {
                    Console.WriteLine("ERROR: No se encontró la descripción del perfil");
                    return null;
                }

                Console.WriteLine($"\n8. Buscando roles para perfil {usuarioPerfil}");
                List<String> rolesData = _dataBaseUtils.BuscarRegistrosPorValor("perfil_rol.csv", usuarioPerfil.ToString(), 0);
                Console.WriteLine($"Roles encontrados: {rolesData.Count}");
                
                List<Rol> roles = new List<Rol>();
                foreach (String rolData in rolesData)
                {
                    Console.WriteLine($"\nProcesando rol: {rolData}");
                    String[] camposRol = rolData.Split(';');
                    Console.WriteLine($"Número de campos en rol: {camposRol.Length}");
                    Console.WriteLine($"Campos: [{string.Join(" | ", camposRol)}]");
                    
                    // Verificamos que no sea la línea de encabezado
                    if (camposRol[0] == "idPerfil" || camposRol[0] == "id")
                    {
                        Console.WriteLine("Saltando línea de encabezado");
                        continue;
                    }
                    
                    if (camposRol.Length >= 2)
                    {
                        Console.WriteLine($"Intentando parsear idRol: {camposRol[1]}");
                        int idRol = Int32.Parse(camposRol[1]);
                        Console.WriteLine($"ID de rol parseado: {idRol}");
                        
                        Console.WriteLine($"Buscando descripción del rol {idRol}");
                        String denRol = _dataBaseUtils.BuscarValor("rol.csv", idRol.ToString(), 0, 1);
                        Console.WriteLine($"Descripción del rol encontrada: {denRol}");
                        
                        if (!string.IsNullOrEmpty(denRol))
                        {
                            roles.Add(new Rol { IdRol = idRol, Codigo = denRol, Descripcion = denRol });
                            Console.WriteLine($"Rol agregado - ID: {idRol}, Código: {denRol}, Descripción: {denRol}");
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: No se encontró información para el rol {idRol}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: formato incorrecto en rol {rolData}");
                    }
                }

                Console.WriteLine("\n9. Creando objeto Credencial");
                Console.WriteLine($"Parámetros: strCredencial={strCredencial}, intentos={intentos}, bloqueo={bloqueo}, descripcionPerfil={descripcionPerfil}, roles={roles.Count}");
                Credencial credencial = new Credencial(strCredencial, intentos, bloqueo, descripcionPerfil, roles);
                Console.WriteLine("Objeto Credencial creado");
                
                Console.WriteLine($"\n10. Asignando ID de perfil: {usuarioPerfil}");
                credencial.IdPerfil = usuarioPerfil;
                Console.WriteLine($"Credencial creada con ID de perfil: {credencial.IdPerfil}");
                
                Console.WriteLine("\n=== FIN ObtenerPorUsuario ===");
                return credencial;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR CRÍTICO en ObtenerPorUsuario:");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"TargetSite: {ex.TargetSite}");
                throw;
            }
        }

        public void Actualizar(Credencial credencial)
        {
            _dataBaseUtils.ModificarRegistro(_archivoCredenciales, 1, credencial.Usuario, credencial.ToStringCSV());
        }

        public Credencial ValidarCredencial(String usuario, String password)
        {
            Console.WriteLine($"\n=== Validando credencial para usuario: {usuario} ===");
            
            try
            {
                Console.WriteLine("\n1. Buscando credenciales...");
                List<String> lstCredenciales = _dataBaseUtils.BuscarRegistrosPorValor(_archivoCredenciales, usuario, 1);
                
                if (lstCredenciales.Count == 0)
                {
                    Console.WriteLine("No se encontraron credenciales para el usuario");
                    return null;
                }

                Console.WriteLine($"\n2. Procesando credencial encontrada: {lstCredenciales[0]}");
                String[] camposCredencial = lstCredenciales[0].Split(';');
                Console.WriteLine($"Número de campos en credencial: {camposCredencial.Length}");
                Console.WriteLine($"Campos: [{string.Join(" | ", camposCredencial)}]");

                if (camposCredencial.Length < 5)
                {
                    Console.WriteLine("ERROR: La credencial no tiene suficientes campos");
                    return null;
                }

                string legajo = camposCredencial[0];
                string nombreUsuario = camposCredencial[1];
                string passwordHash = camposCredencial[2];
                string fechaAlta = camposCredencial[3];
                string fechaBaja = camposCredencial[4];
                string estado = camposCredencial.Length > 5 ? camposCredencial[5] : "";

                Console.WriteLine($"\n3. Datos de credencial:");
                Console.WriteLine($"- Legajo: {legajo}");
                Console.WriteLine($"- Usuario: {nombreUsuario}");
                Console.WriteLine($"- Password Hash: {passwordHash}");
                Console.WriteLine($"- Fecha Alta: {fechaAlta}");
                Console.WriteLine($"- Fecha Baja: {fechaBaja}");
                Console.WriteLine($"- Estado: {estado}");

                Console.WriteLine("\n4. Verificando intentos de login...");
                List<String> lstIntentos = _dataBaseUtils.BuscarRegistrosPorValor("login_intentos.csv", legajo, 0);
                Console.WriteLine($"Intentos de login encontrados: {lstIntentos.Count}");

                Console.WriteLine("\n5. Verificando si el usuario está bloqueado...");
                List<String> lstBloqueos = _dataBaseUtils.BuscarRegistrosPorValor("usuario_bloqueado.csv", legajo, 0);
                Console.WriteLine($"Bloqueos encontrados: {lstBloqueos.Count}");

                Console.WriteLine("\n6. Obteniendo perfil del usuario...");
                String idPerfil = _dataBaseUtils.BuscarValor(_archivoUsuarioPerfil, legajo, 0, 1);
                Console.WriteLine($"ID de perfil encontrado: {idPerfil}");

                if (string.IsNullOrEmpty(idPerfil))
                {
                    Console.WriteLine("ERROR: No se encontró perfil para el usuario");
                    return null;
                }

                Console.WriteLine("\n7. Obteniendo descripción del perfil...");
                String descripcionPerfil = _dataBaseUtils.BuscarValor("perfil.csv", idPerfil, 0, 1);
                Console.WriteLine($"Descripción de perfil encontrada: {descripcionPerfil}");

                if (string.IsNullOrEmpty(descripcionPerfil))
                {
                    Console.WriteLine("ERROR: No se encontró descripción para el perfil");
                    return null;
                }

                Console.WriteLine("\n8. Obteniendo roles del perfil...");
                List<String> lstRoles = _dataBaseUtils.BuscarRegistrosPorValor("perfil_rol.csv", idPerfil, 0);
                Console.WriteLine($"Roles encontrados: {lstRoles.Count}");

                List<Rol> roles = new List<Rol>();
                foreach (String rol in lstRoles)
                {
                    Console.WriteLine($"\nProcesando rol: {rol}");
                    String[] camposRol = rol.Split(';');
                    Console.WriteLine($"Número de campos en rol: {camposRol.Length}");
                    Console.WriteLine($"Campos: [{string.Join(" | ", camposRol)}]");

                    if (camposRol.Length < 2)
                    {
                        Console.WriteLine("ERROR: El rol no tiene suficientes campos");
                        continue;
                    }

                    String idRol = camposRol[1];
                    Console.WriteLine($"Buscando descripción para rol ID: {idRol}");
                    String denRol = _dataBaseUtils.BuscarValor("rol.csv", idRol, 0, 1);
                    
                    if (!string.IsNullOrEmpty(denRol))
                    {
                        Console.WriteLine($"Descripción de rol encontrada: {denRol}");
                        roles.Add(new Rol { IdRol = int.Parse(idRol), Codigo = denRol, Descripcion = denRol });
                    }
                    else
                    {
                        Console.WriteLine("ERROR: No se encontró descripción para el rol");
                    }
                }

                Console.WriteLine($"\n9. Roles encontrados: {string.Join(", ", roles.Select(r => r.Descripcion))}");

                if (passwordHash.Equals(password))
                {
                    Console.WriteLine("\n10. Password correcto - creando objeto Credencial");
                    string strCredencial = $"{legajo};{nombreUsuario};{passwordHash};{fechaAlta};{fechaBaja};{estado}";
                    return new Credencial(strCredencial, lstIntentos.Count, lstBloqueos.Count > 0, descripcionPerfil, roles);
                }
                else
                {
                    Console.WriteLine("\n10. Password incorrecto");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERROR CRÍTICO en ValidarCredencial:");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"TargetSite: {ex.TargetSite}");
                return null;
            }
        }
    }
} 