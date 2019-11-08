using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ElecLibrary
{
    public static class CsvFile
    {
        public static Acta GetActa(string rowActaCsv, string repoCsvName = "_")
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length > 26 || fields.Length < 25)
                return null;
            
            var nameKeys = Path.GetFileNameWithoutExtension(repoCsvName);
            var origenFecha = nameKeys.Split('_');

            Acta acta = new Acta
            {
                Pais = fields[0],
                NumeroDepartamento = fields[1],
                Departamento = fields[2],
                Provincia = fields[3],
                NumeroMunicipio = fields[4],
                Municipio = fields[5],
                Circunscripcion = fields[6],
                Localidad = fields[7],
                Recinto = fields[8],
                NumeroMesa = fields[9],
                CodigoMesa = fields[10],
                Eleccion = fields[11],
                Inscritos = fields[12],
                CC = fields[13],
                FPV = fields[14],
                MTS = fields[15],
                UCS = fields[16],
                MAS_IPSP = fields[17],
                F21 = fields[18],
                PDC = fields[19],
                MNR = fields[20],
                PAN_BOL = fields[21],
                VotosValidos = fields[22],
                Blancos = fields[23],
                Nulos = fields[24],
                EstadoActa = fields.Length > 25 ? fields[25] : "",
                Origen = origenFecha.Length >= 2 ? origenFecha[0] : "",
                Fecha =  origenFecha.Length >= 2 ? origenFecha[1] : ""
            };

            return acta;    
        }

        public static Acta GetActaComp(string rowActaCsv, string repoCsvName = "_")
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length != 26)
                return null;
            
            var nameKeys = Path.GetFileNameWithoutExtension(repoCsvName);
            var origenFecha = nameKeys.Split('_');

            Acta acta = new Acta
            {
                Pais = fields[0],
                NumeroDepartamento = fields[1],
                Departamento = fields[2],
                Provincia = fields[3],
                NumeroMunicipio = fields[4],
                Municipio = fields[5],
                Circunscripcion = fields[6],
                Localidad = fields[7],
                Recinto = fields[8],
                NumeroMesa = fields[9],
                CodigoMesa = fields[10],
                Eleccion = fields[11],
                Inscritos = fields[12],
                CC = fields[13],
                FPV = fields[14],
                MTS = fields[15],
                UCS = fields[16],
                MAS_IPSP = fields[17],
                F21 = fields[18],
                PDC = fields[19],
                MNR = fields[20],
                PAN_BOL = fields[21],
                VotosValidos = fields[22],
                Blancos = fields[23],
                Nulos = fields[24],
                EstadoActa = fields.Length > 25 ? fields[25] : "",
                Origen = origenFecha.Length >= 2 ? origenFecha[0] : "",
                Fecha =  origenFecha.Length >= 2 ? origenFecha[1] : ""
            };

            return acta;    
        }
        public static Acta GetActaTrep(string rowActaCsv, string repoCsvName = "_")
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length != 25)
                return null;
            
            var nameKeys = Path.GetFileNameWithoutExtension(repoCsvName);
            var origenFecha = nameKeys.Split('_');

            Acta acta = new Acta
            {
                Pais = fields[0],
                NumeroDepartamento = fields[1],
                Departamento = fields[2],
                Provincia = fields[3],
                NumeroMunicipio = fields[4],
                Municipio = fields[5],
                Circunscripcion = fields[6],
                Localidad = fields[7],
                Recinto = fields[8],
                NumeroMesa = fields[9],
                CodigoMesa = fields[10],
                Eleccion = fields[11],
                Inscritos = fields[12],
                CC = fields[13],
                FPV = fields[14],
                MTS = fields[15],
                UCS = fields[16],
                MAS_IPSP = fields[17],
                F21 = fields[18],
                PDC = fields[19],
                MNR = fields[20],
                PAN_BOL = fields[21],
                VotosValidos = fields[22],
                Blancos = fields[23],
                Nulos = fields[24],
                EstadoActa = fields.Length > 25 ? fields[25] : "",
                Origen = origenFecha.Length >= 2 ? origenFecha[0] : "",
                Fecha =  origenFecha.Length >= 2 ? origenFecha[1] : ""
            };

            return acta;    
        }
        public static PgActaComp GetPgActaComp(string rowActaCsv, string repoCsvName = "_")
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length != 26)
                return null;
            
            var acta = new PgActaComp
            {
                pais = fields[0],
                numero_departamento = int.Parse(fields[1]),
                departamento = fields[2],
                provincia = fields[3],
                numero_municipio = int.Parse(fields[4]),
                municipio = fields[5],
                circunscripcion = fields[6],
                localidad = fields[7],
                recinto = fields[8],
                numero_mesa = int.Parse(fields[9]),
                codigo_mesa = long.Parse(fields[10]),
                eleccion = fields[11],
                inscritos = int.Parse(fields[12]),
                cc = int.Parse(fields[13]),
                fpv = int.Parse(fields[14]),
                mts = int.Parse(fields[15]),
                ucs = int.Parse(fields[16]),
                mas_ipsp = int.Parse(fields[17]),
                _21f = int.Parse(fields[18]),
                pdc = int.Parse(fields[19]),
                mnr = int.Parse(fields[20]),
                pan_bol = int.Parse(fields[21]),
                votos_validos = int.Parse(fields[22]),
                blancos = int.Parse(fields[23]),
                nulos = int.Parse(fields[24]),
                estado_acta = fields[25]
            };

            return acta;    
        }
        public static PgActaTrep GetPgActaTrep(string rowActaCsv, string repoCsvName = "_")
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length != 25)
                return null;
            
            var acta = new PgActaTrep();
                acta.pais = fields[0];
                acta.numero_departamento = int.Parse(fields[1]);
                acta.departamento = fields[2];
                acta.provincia = fields[3];
                acta.numero_municipio = int.Parse(fields[4]);
                acta.municipio = fields[5];
                acta.circunscripcion = fields[6];
                acta.localidad = fields[7];
                acta.recinto = fields[8];
                acta.numero_mesa = int.Parse(fields[9]);
                acta.codigo_mesa = long.Parse(fields[10]);
                acta.eleccion = fields[11];
                acta.inscritos = int.Parse(fields[12]);
                acta.cc = int.Parse(fields[13]);
                acta.fpv = int.Parse(fields[14]);
                acta.mts = int.Parse(fields[15]);
                acta.ucs = int.Parse(fields[16]);
                acta.mas_ipsp = int.Parse(fields[17]);
                acta._21f = int.Parse(fields[18]);
                acta.pdc = int.Parse(fields[19]);
                acta.mnr = int.Parse(fields[20]);
                acta.pan_bol = int.Parse(fields[21]);
                acta.votos_validos = int.Parse(fields[22]);
                acta.blancos = int.Parse(fields[23]);
                acta.nulos = int.Parse(fields[24]);

            return acta;    
        }
        public static string ReadAll(string fileName)
        {
            var memFile = File.ReadAllText(fileName);
            return memFile;
        }

        public static IEnumerable<string> ReadAllLines(string fileName, 
                                                        int firstLinea = 1, 
                                                        int lastLinea = int.MaxValue)
        {
            var lines = new List<string>();
            using(StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                int i = 1;
                while(line != null )
                {
                    if(i >= firstLinea && i <= lastLinea)
                        lines.Add(line);
                    i++;
                    line = sr.ReadLine();
                }
            }

            return lines; 
        }

        public static string ReadHeader(string fileName)
        {
            using(StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                return line;
            } 
        }
        public static IEnumerable<string> ReadHeaderFields(string fileName)
        {
            var header = CsvFile.ReadHeader(fileName);
            var headerFields = header.Split(',');
            return headerFields;
        }
        public static IEnumerable<string> GetAllFilenames(string path, string ext = null)
        {
            string searchPatron = ext != null ? $"*.{ext}" : null;
            IEnumerable<string> result = null;
            if(searchPatron != null)
                result = Directory.EnumerateFiles(path, searchPatron);
            else
                result = Directory.EnumerateFiles(path);
            return result;
        }
        
        public static (int validos, int total) CountCsvComputoFiles(string pathCsvFile)
        {
            int total = 0;
            int validos = 0;
            var files = GetAllFilenames(pathCsvFile, "*.csv");
            foreach (var file in files)
            {
                total++;
                if(IsCsvComputo(file))
                    validos++;

            }
            return(validos, total);
        }
        public static (int validos, int total) CountCsvTrepFiles(string pathCsvFile)
        {
            int total = 0;
            int validos = 0;
            var files = GetAllFilenames(pathCsvFile, "*.csv");
            foreach (var file in files)
            {
                total++;
                if(IsCsvTrep(file))
                    validos++;

            }
            return(validos, total);
        }
        public static (int eliminados, int total) DeleteCsvTrepFilesInvalidos(string pathCsvFile)
        {
            int total = 0;
            int eliminados = 0;
            var files = GetAllFilenames(pathCsvFile, "*.csv");
            foreach (var file in files)
            {
                total++;
                if(!IsCsvTrep(file))
                {
                    File.Delete(file);
                    eliminados++;
                }

            }
            return(eliminados, total);
        }
        public static bool IsCsvComputo(string pathCsvFile)
        {
            string header = ReadHeader(pathCsvFile);
            var partes = header.Split(',');
            return partes.Length == 26;
        }
        public static bool IsCsvTrep(string pathCsvFile)
        {
            string header = ReadHeader(pathCsvFile);
            var partes = header.Split(',');
            return partes.Length == 25;
        }


        public static IEnumerable<string> ExcelToCSV(string excelPath)
        {
            //create a list to hold all the values
            List<string> excelData = new List<string>();

            //read the Excel file as byte array
            byte[] bin = File.ReadAllBytes(excelPath);

            //create a new Excel package in a memorystream
            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {   
                ExcelWorksheet worksheet = null;
                try
                {
                    //first worksheet
                    worksheet = excelPackage.Workbook.Worksheets[1];
                }
                catch (System.Exception)
                {
                    return null;
                }

                //loop all rows
                for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                {
                    string row = "";
                    
                    //loop all columns in a row
                    for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                    {
                        //add the cell data to the List
                        row += worksheet.Cells[i, j].Value != null ? worksheet.Cells[i, j].Value.ToString() : "";
                        if(j < worksheet.Dimension.End.Column)
                            row += ",";
                    }

                    excelData.Add(row);
                }

                return excelData;
            }
        }
        public static void ExcelToCSVFile(string excelPath, string csvPath)
        {
            var lineasCsv = ExcelToCSV(excelPath);
            if(lineasCsv == null)
            {
                System.Console.WriteLine($"*** Archivo null: {excelPath}");
                return;
            } 
            if(lineasCsv.Count() == 0)
            {
                System.Console.WriteLine($"*** Archivo vacio: {excelPath}");
                return;
            }

            using(var writer = new StreamWriter(csvPath, append:false))
            {
                foreach (var linea in lineasCsv)
                {
                    writer.WriteLine(linea);
                }
            }
        }
    }
}
