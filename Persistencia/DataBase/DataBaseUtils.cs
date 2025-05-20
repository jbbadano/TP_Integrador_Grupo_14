using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Persistencia.DataBase
{
    public class DataBaseUtils
    {
        private static string rutaBaseArchivos;

        public DataBaseUtils()
        {
            rutaBaseArchivos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "Tablas");
            Console.WriteLine($"Ruta base de archivos: {rutaBaseArchivos}");
        }

        public List<String> ObtenerTodosLosRegistros(string nombreArchivo)
        {
            string rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
            Console.WriteLine($"Leyendo archivo: {rutaArchivo}");
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine($"Archivo no encontrado: {rutaArchivo}");
                return new List<String>();
            }

            return File.ReadAllLines(rutaArchivo).ToList();
        }

        // Método para buscar un registro con un valor específico
        public List<String> BuscarRegistrosPorValor(string nombreArchivo, string valor, int indiceColumna)
        {
            Console.WriteLine($"\n=== Buscando en archivo: {nombreArchivo} ===");
            Console.WriteLine($"Buscando valor '{valor}' en columna {indiceColumna}");
            
            List<String> lstResultado = new List<String>();
            string rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
            
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine($"ERROR: Archivo no encontrado: {rutaArchivo}");
                return lstResultado;
            }

            Console.WriteLine("Leyendo todas las líneas del archivo...");
            string[] lineas = File.ReadAllLines(rutaArchivo);
            Console.WriteLine($"Total de líneas en archivo: {lineas.Length}");
            
            bool primeraLinea = true;
            foreach (string linea in lineas)
            {
                Console.WriteLine($"\nProcesando línea: {linea}");
                String[] campos = linea.Split(';');
                Console.WriteLine($"Número de campos en línea: {campos.Length}");
                Console.WriteLine($"Campos: [{string.Join(" | ", campos)}]");

                if (primeraLinea)
                {
                    Console.WriteLine("Primera línea detectada - verificando si es encabezado");
                    if (campos.Length > 0 && (campos[0].ToLower().Contains("id") || campos[0].ToLower().Contains("perfil")))
                    {
                        Console.WriteLine("Es encabezado - saltando línea");
                        primeraLinea = false;
                        continue;
                    }
                    primeraLinea = false;
                }

                if (campos.Length <= indiceColumna)
                {
                    Console.WriteLine($"ERROR: Línea no tiene suficientes columnas. Se requieren {indiceColumna + 1}, tiene {campos.Length}");
                    continue;
                }

                Console.WriteLine($"Comparando: '{campos[indiceColumna]}' con '{valor}'");
                if (campos[indiceColumna].Equals(valor))
                {
                    Console.WriteLine("¡Coincidencia encontrada!");
                    lstResultado.Add(linea);
                }
            }

            Console.WriteLine($"Total de coincidencias encontradas: {lstResultado.Count}");
            if (lstResultado.Count > 0)
            {
                Console.WriteLine("Coincidencias:");
                foreach (var resultado in lstResultado)
                {
                    Console.WriteLine($"- {resultado}");
                }
            }
            
            Console.WriteLine("=== FIN BuscarRegistrosPorValor ===");
            return lstResultado;
        }

        // Método para buscar un valor en una columna específica
        public String BuscarValor(String nombreArchivo, String valor, int columnaOrigen, int columnaDestino)
        {
            Console.WriteLine($"\n=== Buscando valor en archivo: {nombreArchivo} ===");
            Console.WriteLine($"Buscando '{valor}' en columna {columnaOrigen}, retornando valor de columna {columnaDestino}");
            
            String rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
            
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine($"ERROR: Archivo no encontrado: {rutaArchivo}");
                return null;
            }

            String[] lineas = File.ReadAllLines(rutaArchivo);
            Console.WriteLine($"Total de líneas en archivo: {lineas.Length}");
            
            bool primeraLinea = true;
            foreach (String linea in lineas)
            {
                Console.WriteLine($"\nProcesando línea: {linea}");
                String[] campos = linea.Split(';');
                Console.WriteLine($"Número de campos en línea: {campos.Length}");
                Console.WriteLine($"Campos: [{string.Join(" | ", campos)}]");

                if (primeraLinea)
                {
                    Console.WriteLine("Primera línea detectada - verificando si es encabezado");
                    if (campos.Length > 0 && (campos[0].ToLower().Contains("id") || campos[0].ToLower().Contains("perfil")))
                    {
                        Console.WriteLine("Es encabezado - saltando línea");
                        primeraLinea = false;
                        continue;
                    }
                    primeraLinea = false;
                }

                if (campos.Length <= Math.Max(columnaOrigen, columnaDestino))
                {
                    Console.WriteLine($"ERROR: Línea no tiene suficientes columnas. Se requieren {Math.Max(columnaOrigen, columnaDestino) + 1}, tiene {campos.Length}");
                    continue;
                }

                Console.WriteLine($"Comparando: '{campos[columnaOrigen]}' con '{valor}'");
                if (campos[columnaOrigen].Equals(valor))
                {
                    Console.WriteLine($"¡Coincidencia encontrada! Retornando valor: {campos[columnaDestino]}");
                    return campos[columnaDestino];
                }
            }

            Console.WriteLine("No se encontró coincidencia");
            return null;
        }

        // Método para agregar un registro
        public void AgregarRegistro(string nombreArchivo, string registro)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine($"Archivo no encontrado: {rutaArchivo}");
                    return;
                }

                File.AppendAllText(rutaArchivo, registro + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarRegistro: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        // Método para modificar un registro
        public void ModificarRegistro(string nombreArchivo, int indiceColumna, string valorBusqueda, string nuevoRegistro)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine($"Archivo no encontrado: {rutaArchivo}");
                    return;
                }

                string[] lineas = File.ReadAllLines(rutaArchivo);
                List<string> nuevasLineas = new List<string>();

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                    {
                        continue;
                    }

                    string[] campos = linea.Split(';');
                    if (campos.Length > indiceColumna && campos[indiceColumna] == valorBusqueda)
                    {
                        nuevasLineas.Add(nuevoRegistro);
                    }
                    else
                    {
                        nuevasLineas.Add(linea);
                    }
                }

                File.WriteAllLines(rutaArchivo, nuevasLineas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ModificarRegistro: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        // Elimina uno o varios registros buscando un valor en una columna
        public void EliminarRegistros(string nombreArchivo, int indiceColumna, string valor)
        {
            try
            {
                string rutaArchivo = Path.Combine(rutaBaseArchivos, nombreArchivo);
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine($"Archivo no encontrado: {rutaArchivo}");
                    return;
                }

                string[] lineas = File.ReadAllLines(rutaArchivo);
                List<string> nuevasLineas = new List<string>();

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                    {
                        continue;
                    }

                    string[] campos = linea.Split(';');
                    if (campos.Length > indiceColumna && campos[indiceColumna] != valor)
                    {
                        nuevasLineas.Add(linea);
                    }
                }

                File.WriteAllLines(rutaArchivo, nuevasLineas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarRegistros: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        public void EliminarRegistrosBKP(String nombreArchivo, int col_id, String valor_id)
        {
            String rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Datos", nombreArchivo);

            FileInfo fi = new FileInfo(rutaArchivo);
            if (!fi.Exists)
            {
                Console.WriteLine("El archivo no existe: " + nombreArchivo);
                return;
            }
            else
            {
                StreamReader sr = fi.OpenText();

                // Leo el archivo y lo guardo en una lista sin el/los registros que coincidan
                List<String> listado = new List<String>();
                while (!sr.EndOfStream)
                {
                    String[] datos = sr.ReadLine().ToString().Split(';');
                    if (datos[col_id] != valor_id)
                    {
                        listado.Add(String.Join(";", datos));
                    }
                }
                sr.Close();

                // Escribo el archivo con el registro modificado
                StreamWriter sw = fi.CreateText();
                foreach (String item in listado)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
            }

        }

    }
}
