using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ElecLibrary
{
    public static class CsvFile
    {
        public static Acta GetActa(string rowActaCsv)
        {
            if(string.IsNullOrWhiteSpace(rowActaCsv))
                return null;
            string[] fields = rowActaCsv.Split(',');
            if(fields.Length != 26)
                return null;

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
                EstadoActa = fields[25]
            };

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
                //first worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

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
            if(lineasCsv == null || lineasCsv.Count() == 0)
                return;

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
