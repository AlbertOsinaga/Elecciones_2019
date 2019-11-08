using System;
using System.IO;
using ElecLibrary;

namespace ElecConsole
{
    class Program
    {
        #region Properties
        static string StrCnn = "Server=durkteel.cbtk6v6dl75j.us-east-1.rds.amazonaws.com;" + 
                                "Port=5432;Database=durkteel;" + 
                                "User id=laos;Password=KP/QU3rT*VmmR2TY";
        static StreamWriter Writer = null;
        static string PathWriter = "null";
        static bool WriterIsOpen = false;
        #endregion

        static void Main(string[] args)
        {
            System.Console.WriteLine();
            Console.WriteLine("Programa 'Elecciones 2019'");
            System.Console.WriteLine();

            DateTime inicio = DateTime.Now;
            System.Console.WriteLine($"Inicio proceso: {inicio}");
            System.Console.WriteLine();


            // InsertTrepCsvToPg("/Volumes/easystore/Dropbox/EReportes/Trep/trep_2019.10.20.11.34.57.csv");
            // CopyCsvCompToPg("/Volumes/easystore/Dropbox/EReportes/Computo/trep_2019.10.25.21.06.30.csv");
            InsertCompCsvToPg("/Volumes/easystore/Dropbox/EReportes/Computo/trep_2019.10.25.21.06.30.csv");

            DateTime final = DateTime.Now;
            System.Console.WriteLine();
            System.Console.WriteLine($"Fin de proceso: {final}");
            TimeSpan duracion = final - inicio;
            Console.WriteLine($"Duracion: {duracion.ToString()}");
            System.Console.WriteLine();
        }

        #region Methods

        static void FilesCsvList(string path,  bool withoutExtension = true)
        {
            FilesList(path, "*.csv", withoutExtension);
        }
        static void FilesExcelList(string path,  bool withoutExtension = true)
        {
            FilesList(path, "*.xlsx", withoutExtension);
        }
        static void FilesList(string path, string extension, bool withoutExtension = true)
        {
            var files = CsvFile.GetAllFilenames(path, extension);
            int i = 0;
            foreach (var file in files)
            {
                string fileName = withoutExtension ? 
                                    Path.GetFileNameWithoutExtension(file) :
                                    Path.GetFileName(file);
                System.Console.WriteLine($"{++i}) {file} - {fileName}");
            }
        }
        static void FilesExcelToCsv(string excelPath, string csvPath)
        {
            var filesXlsx = CsvFile.GetAllFilenames(excelPath, "*.xlsx");
            int i = 0;
            foreach (var filex in filesXlsx)
            {
                string fileName = Path.GetFileNameWithoutExtension(filex);
                string fileNameX = Path.GetFileName(filex);
                string filec = Path.Combine(csvPath, fileName + ".csv");
                string fileNameC = Path.GetFileName(filec);

                if(!File.Exists(filec))
                {
                    System.Console.WriteLine($"{++i}) {fileNameX} - {fileNameC}");
                    CsvFile.ExcelToCSVFile(filex, filec);
                    // System.Console.WriteLine($"{++i}) {filex} - {fileName} - {filec}");
                }
            }
        }
        static void CopyCsvCompToPg(string csvPath, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(csvPath);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_comp_" + tableName;
            tableName = tableName.Replace('.', '_');
            using(ElecPgRepository dbRepo = new ElecPgRepository(StrCnn))
            {
                dbRepo.CreatePgActaCompTable(tableName, schema);

                PathWriter = $"/Volumes/easystore/Dropbox/Temp/copy_{tableName}.txt";
                WriterOpen();
                WriteLine($"Database: durkteel");
                WriteLine($"Tabla: {schema}.{tableName}");
                WriteLine($"Copiando reporte: {csvPath}");
                WriteLine("--------------------------------------------------------------");

                int ret = dbRepo.CopyCsvCompToPg(csvPath, tableName);
                WriteLine($"Filas insertadas: {ret}");
                WriteLine("");
                WriterClose();
            }
        }
        static void InsertAllReposCsvToDB(string dbPath, string csvPath)
        {
            var fileNames = CsvFile.GetAllFilenames(csvPath, "csv");
            foreach (var file in fileNames)
            {
                var table = "Acta_" + Path.GetFileNameWithoutExtension(file);
                table = table.Replace('.', '_');
                InsertRepoCsvToDB(dbPath, table, file);
            }
        }
        static void InsertRepoCsvToDB(string dbPath, string tableName, string repoCsvName)
        {
            var lines = CsvFile.ReadAllLines(repoCsvName, 2);
            System.Console.WriteLine($"Database: {dbPath}");
            System.Console.WriteLine($"Tabla: {tableName}");
            System.Console.WriteLine($"Insertando reporte: {repoCsvName}");
            System.Console.WriteLine("--------------------------------------------------------------");
            var dbRepo = new ElecRepository(dbPath);
            dbRepo.CreateSQLiteActasTable(tableName);
            foreach (var linea in lines)
            {
                Acta acta = CsvFile.GetActa(linea, repoCsvName);
                long actaId = dbRepo.InsertActa(acta, tableName);
                System.Console.WriteLine($"Acta insertada: {actaId}");
            }
            System.Console.WriteLine();
        }
        static void InsertCompCsvToPg(string compCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(compCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_comp_" + tableName;
            tableName = tableName.Replace('.', '_');
            PathWriter = $"/Volumes/easystore/Dropbox/Temp/{tableName}.txt";
            WriterOpen();
            WriteLine($"Database: durkteel");
            WriteLine($"Tabla: {schema}.{tableName}");
            WriteLine($"Insertando reporte: {compCsvName}");
            WriteLine("--------------------------------------------------------------");
            using(ElecPgRepository dbRepo = new ElecPgRepository(StrCnn))
            {
                dbRepo.CreatePgActaCompTable(tableName, schema);
                var lines = CsvFile.ReadAllLines(compCsvName, 2);
                int i = 0;
                foreach (var linea in lines)
                {
                    PgActaComp acta = CsvFile.GetPgActaComp(linea, compCsvName);
                    long ret = dbRepo.InsertPgActaComp(acta, tableName, schema);
                    if(ret == 1)
                        WriteLine($"Acta insertada: {++i}");
                    else
                        WriteLine($"Acta no insertada (ret={ret}): {++i}");
                }
                WriteLine("");
                WriterClose();
            }
        }
        static void InsertTrepCsvToPg(string trepCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(trepCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_trep_" + tableName;
            tableName = tableName.Replace('.', '_');
            PathWriter = $"/Volumes/easystore/Dropbox/Temp/{tableName}.txt";
            WriterOpen();
            WriteLine($"Database: durkteel");
            WriteLine($"Tabla: {schema}.{tableName}");
            WriteLine($"Insertando reporte: {trepCsvName}");
            WriteLine("--------------------------------------------------------------");
            var lines = CsvFile.ReadAllLines(trepCsvName, 2);
            int i = 0;
            using(ElecPgRepository dbRepo = new ElecPgRepository(StrCnn))
            {
                dbRepo.CreatePgActaTrepTable(tableName, schema);
                foreach (var linea in lines)
                {
                    PgActaTrep acta = CsvFile.GetPgActaTrep(linea, trepCsvName);
                    long ret = dbRepo.InsertPgActaTrep(acta, tableName, schema);
                    if(ret == 1)
                        WriteLine($"Acta insertada: {++i}");
                    else
                        WriteLine($"Acta no insertada (ret={ret}): {++i}");
                }
                WriteLine("");
                WriterClose();
            }
        }
        static void ListAllActas(string dbPath, string tableName)
        {
            var dbRepo = new ElecRepository(dbPath);
            System.Console.WriteLine($"Database: {dbPath}");
            System.Console.WriteLine($"Tabla: {tableName}");
            System.Console.WriteLine($"Lista de Actas");
            System.Console.WriteLine("--------------------------------------------------------------");
            var actas = dbRepo.GetAllActas(tableName);
            foreach (var acta in actas)
            {
                System.Console.WriteLine(acta);
                System.Console.WriteLine();
            }       
        }
        static void Garbage()
        {
            // InsertAllReposCsvToDB("/Volumes/easystore/Dropbox/EReportes/eleccion.db", 
            //                         "/Volumes/easystore/Dropbox/EReportes/Trep");

            // ListAllActas("/Volumes/easystore/Dropbox/EReportes/elec.db", 
            //                     "Actas_trep_2019_10_20_11_34_57");

            // InsertRepoCsvToDB("/Volumes/easystore/Dropbox/EReportes/elec.db", 
            //                     "Actas_trep_2019_10_20_11_34_57", 
            //                     "/Volumes/easystore/Dropbox/EReportes/Trep/trep_2019.10.20.11.34.57.csv");

            // FilesCsvList("/Volumes/easystore/Dropbox/EReportes/Computo", withoutExtension:false);
            // FilesCsvList("/Volumes/easystore/Dropbox/EReportes/Trep", withoutExtension:false);

            // FilesExcelToCsv("/Volumes/easystore/Dropbox/Reports/Computo", 
            //                         "/Volumes/easystore/Dropbox/EReportes/Computo");

            // var result = CsvFile.CountCsvComputoFiles("/Volumes/easystore/Dropbox/EReportes/Computo");
            // System.Console.WriteLine($"Validos = {result.validos}, Total = {result.total}");

            // var result = CsvFile.CountCsvTrepFiles("/Volumes/easystore/Dropbox/EReportes/Trep");
            // System.Console.WriteLine($"Validos = {result.validos}, Total = {result.total}");

            // var result = CsvFile.DeleteCsvTrepFilesInvalidos("/Volumes/easystore/Dropbox/EReportes/Trep");
            // System.Console.WriteLine($"Eliminados = {result.eliminados}, Total = {result.total}");

            // string strCnn = "Server=durkteel.cbtk6v6dl75j.us-east-1.rds.amazonaws.com;" + 
            //                 "Port=5432;Database=durkteel;" + 
            //                 "User id=laos;Password=KP/QU3rT*VmmR2TY";
            // ElecPgRepository dbRepo = new ElecPgRepository(strCnn);
            // dbRepo.CreatePgActaTrepTable();                

            // FilesExcelToCsv("/Volumes/easystore/Dropbox/Reports/Errados/T", 
            //                         "/Volumes/easystore/Dropbox/EReportes/Trep");

            // var name = Path.GetFileNameWithoutExtension("/Volumes/easystore/Dropbox/EReportes/Trep/trep_2019.10.20.11.34.57.csv");
            // var partes = name.Split('_');
            // if(partes.Length >= 2)
            // {
            //     System.Console.WriteLine($"Origen: {partes[0]}");
            //     System.Console.WriteLine($"Fecha: {partes[1]}");
            // }


            // InsertRepoCsvToDB("/Volumes/easystore/Dropbox/EReportes/elec.db", 
            //                     "Actas_trep_2019_10_20_11_34_57", 
            //                     "/Volumes/easystore/Dropbox/EReportes/Trep/trep_2019.10.20.11.34.57.csv");
     
            // FilesExcelToCsv("/Volumes/easystore/Dropbox/Reports/Computo", 
            //                 "/Volumes/easystore/Dropbox/EReportes/Computo");

            // FilesCsvList("/Volumes/easystore/Dropbox/EReportes/Trep", withoutExtension: false);


            // var fields = CsvFile.ReadHeader("/Volumes/easystore/Dropbox/EReportes/Example.csv");
            // System.Console.WriteLine(fields);

            // var dbRepo = new ElecRepository("/Volumes/easystore/Dropbox/EReportes/elec.db");
            // dbRepo.CreateSQLiteActasTable();

            // var fileNames = CsvFile.GetAllFilenames("/Volumes/easystore/Dropbox/EReportes", "db");
            
            // foreach (var file in fileNames)
            // {
            //     System.Console.WriteLine(file);
            // }

            // var lines = CsvFile.ReadAllLines("/Volumes/easystore/Dropbox/EReportes/ExampleCnv.csv", 2, 11);
            // foreach (var linea in lines)
            // {
            //     Acta acta = CsvFile.GetActa(linea);
            //     System.Console.WriteLine(acta);
            //     System.Console.WriteLine();
            // }

            // var lines = CsvFile.ReadAllLines("/Volumes/easystore/Dropbox/EReportes/Example.csv", 2, 3);
            // foreach (var linea in lines)
            // {
            //     Acta acta = CsvFile.GetActa(linea);
            //     long actaId = dbRepo.InsertActa(acta);
            //     System.Console.WriteLine($"Acta insertada: {actaId}");
            // }
            // System.Console.WriteLine();

            // var actas = dbRepo.GetAllActas();
            // if(actas != null)
            // {        
            //     foreach (var acta in actas)
            //     {
            //         System.Console.WriteLine(acta);
            //     }
            // }
            // else
            // {
            //     System.Console.WriteLine("No se encontraron actas en la db!");
            // }

            // var lineasCsv = CsvFile.ExcelToCSV("/Volumes/easystore/Dropbox/EReportes/Example.xlsx");
            // int i = 1;
            // foreach (var linea in lineasCsv)
            // {
            //     System.Console.WriteLine($"{i++}) {linea}");
            // }

            // CsvFile.ExcelToCSVFile("/Volumes/easystore/Dropbox/EReportes/Example.xlsx",
            //                         "/Volumes/easystore/Dropbox/EReportes/ExampleCnv.csv");
            // System.Console.WriteLine("Archivo convertido!");
        }
        static void WriteLine(string linea)
        {
            if(!WriterIsOpen)
            {
                WriterOpen();
            }
            Writer.WriteLine(linea);
            System.Console.WriteLine(linea);
        }
        static void WriterOpen()
        {
            Writer = new StreamWriter(PathWriter, append:false);
            WriterIsOpen = true;
        }
        static void WriterClose()
        {
            Writer.Close();
            Writer = null;
            WriterIsOpen = false;
        }

        #endregion
    }
}
