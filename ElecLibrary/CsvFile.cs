using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

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
            using(StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                int i = 1;
                while(line != null )
                {
                    if(i >= firstLinea && i <= lastLinea)
                        yield return line;
                    i++;
                    line = sr.ReadLine();
                }
            } 
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
        // static void convertExcelToCSV(string sourceFile, string worksheetName, string targetFile)

        // {

        //     string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\" Excel.0;HDR=Yes;IMEX=1\""; 

        //     OleDbConnection conn = null;

        //     StreamWriter wrtr = null;

        //     OleDbCommand cmd = null;

        //     OleDbDataAdapter da = null; 

        //     try

        //     {

        //         conn = new OleDbConnection(strConn);

        //         conn.Open();

 

        //         cmd = new OleDbCommand("SELECT * FROM [" + worksheetName + "$]", conn);

        //         cmd.CommandType = CommandType.Text;

        //         wrtr = new StreamWriter(targetFile);

 

        //         da = new OleDbDataAdapter(cmd);

        //         DataTable dt = new DataTable();

        //         da.Fill(dt);

 

        //         for (int x = 0; x < dt.Rows.Count; x++)

        //         {

        //             string rowString = "";

        //             for (int y = 0; y < dt.Columns.Count; y++)

        //             {

        //                 rowString += "\"" + dt.Rows[x][y].ToString() + "\",";

        //             }

        //             wrtr.WriteLine(rowString);

        //         }

        //         Console.WriteLine();

        //         Console.WriteLine("Done! Your " + sourceFile + " has been converted into " + targetFile + ".");

        //         Console.WriteLine();

        //     }

        //     catch (Exception exc)

        //     {

        //         Console.WriteLine(exc.ToString());

        //         Console.ReadLine();

        //     }

        //     finally

        //     {

        //         if (conn.State == ConnectionState.Open)

        //         conn.Close();

        //         conn.Dispose();

        //         cmd.Dispose();

        //         da.Dispose();

        //         wrtr.Close();

        //         wrtr.Dispose();

        //     }
        // }
    }
}