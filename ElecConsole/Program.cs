using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
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

            // InsertAllTrepCsvToPg("/Volumes/easystore/Proces");
            // BulkInsertAllTrepCsvToPg("/Volumes/easystore/Proces");
            BulkInsertAllCompCsvToPg("/Volumes/easystore/Proces");

            DateTime final = DateTime.Now;
            System.Console.WriteLine();
            System.Console.WriteLine($"Fin de proceso: {final}");
            TimeSpan duracion = final - inicio;
            Console.WriteLine($"Duracion: {duracion.ToString()}");
            System.Console.WriteLine();
        }

        #region Methods
        static void BulkInsertAllCompCsvToPg(string compCsvPath, string schema = "raw_reports")
        {
            var files = from n in CsvFile.GetAllFilenames(compCsvPath) orderby n 
                            where Path.GetExtension(n) == ".csv" select n;
            System.Console.WriteLine($"*** Insertando csv files from {compCsvPath}");
            System.Console.WriteLine();
            int i = 0;
            foreach (var file in files)
            {
                System.Console.WriteLine($"*** Procesando archivo #{++i} ...");
                BulkInsertCompCsvToPg(file, schema);
                System.Console.WriteLine($"*** Archivo #{i} procesado...");
                System.Console.WriteLine();
            }            
            System.Console.WriteLine($"*** Procesados en total {i} archivos!");            
        }
        static void BulkInsertAllTrepCsvToPg(string trepCsvPath, string schema = "raw_reports")
        {
            var files = from n in CsvFile.GetAllFilenames(trepCsvPath) orderby n 
                            where Path.GetExtension(n) == ".csv" select n;
            System.Console.WriteLine($"*** Insertando csv files from {trepCsvPath}");
            System.Console.WriteLine();
            int i = 0;
            foreach (var file in files)
            {
                System.Console.WriteLine($"*** Procesando archivo #{++i} ...");
                BulkInsertTrepCsvToPg(file, schema);
                System.Console.WriteLine($"*** Archivo #{i} procesado...");
                System.Console.WriteLine();
            }            
            System.Console.WriteLine($"*** Procesados en total {i} archivos!");            
        }
        static void BulkInsertCompCsvToPg(string compCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(compCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_comp_" + tableName;
            tableName = tableName.Replace('.', '_');
            
            PathWriter = $"/Volumes/easystore/Temp/{tableName}.txt";
            WriterOpen();
            WriteLine($"Database: durkteel");
            WriteLine($"Tabla: {schema}.{tableName}");
            WriteLine($"Insertando reporte: {compCsvName}");
            WriteLine("--------------------------------------------------------------");
            
            var lines = CsvFile.ReadAllLines(compCsvName, 2);
            var actas = new List<PgActaComp>();
            foreach (var line in lines)
            {
                PgActaComp acta = CsvFile.GetPgActaComp(line);
                actas.Add(acta);
            }

            using(ElecPgRepository dbRepo = new ElecPgRepository(StrCnn))
            {
                dbRepo.CreatePgActaCompTable(tableName, schema);
                var result = dbRepo.InsertActasComp(actas, tableName, schema);
                WriteLine($"Actas insertadas: {result.Current.Count()}");
                WriterClose();
            }
        }
        static void BulkInsertTrepCsvToPg(string trepCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(trepCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_trep_" + tableName;
            tableName = tableName.Replace('.', '_');
            
            PathWriter = $"/Volumes/easystore/Temp/{tableName}.txt";
            WriterOpen();
            WriteLine($"Database: durkteel");
            WriteLine($"Tabla: {schema}.{tableName}");
            WriteLine($"Insertando reporte: {trepCsvName}");
            WriteLine("--------------------------------------------------------------");
            
            var lines = CsvFile.ReadAllLines(trepCsvName, 2);
            var actas = new List<PgActaTrep>();
            foreach (var line in lines)
            {
                PgActaTrep acta = CsvFile.GetPgActaTrep(line);
                actas.Add(acta);
            }

            using(ElecPgRepository dbRepo = new ElecPgRepository(StrCnn))
            {
                dbRepo.CreatePgActaTrepTable(tableName, schema);
                var result = dbRepo.InsertActasTrep(actas, tableName, schema);
                WriteLine($"Actas insertadas: {result.Current.Count()}");
                WriterClose();
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

                PathWriter = $"/Volumes/easystore/Temp/copy_{tableName}.txt";
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
        static IEnumerable<string> DiffFilesNames(string path1, string path2, string extension, bool withoutExtension = true)
        {
            var fileNamesPath1 = GetFileNames(path1, extension, withoutExtension);
            var fileNamesPath2 = GetFileNames(path2, extension, withoutExtension);
            var fileNamesDiff1 = fileNamesPath1.Where( p => !(fileNamesPath2.Contains(p)));
            // var fileNamesDiff2 = fileNamesPath2.Where( p => !(fileNamesPath1.Contains(p)));
            // fileNamesDiff2.ToList().ForEach(p => p = "* " + p);
            return fileNamesDiff1;
        }
        static void FilesCsvList(string path,  bool withoutExtension = true)
        {
            FilesList(path, "*.csv", withoutExtension);
        }
        static void FilesExcelList(string path,  bool withoutExtension = true)
        {
            FilesList(path, "*.xlsx", withoutExtension);
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
        private static IEnumerable<string> GetAllFilePathsBeginsWith(string directory, string prefix)
        {
            IEnumerable<string> result = null;
            result = Directory.EnumerateFiles(directory, $"{prefix}*.*");
            return result;
        }
        static IEnumerable<string> GetFileNames(string path, string extension, bool withoutExtension = true)
        {
            var listFiles = new List<string>();
            var files = CsvFile.GetAllFilenames(path, extension);
            foreach (var file in files)
            {
                string fileName = withoutExtension ? 
                                    Path.GetFileNameWithoutExtension(file) :
                                    Path.GetFileName(file);
                listFiles.Add(fileName);
            }
            return listFiles;
        }
        static void InsertAllCompCsvToPg(string compCsvPath, string schema = "raw_reports")
        {
            var files = from n in CsvFile.GetAllFilenames(compCsvPath) orderby n 
                            where Path.GetExtension(n) == ".csv" select n;
            System.Console.WriteLine($"***Insertando csv files from {compCsvPath}");
            System.Console.WriteLine();
            int i = 0;
            foreach (var file in files)
            {
                System.Console.WriteLine($"*** Procesando archivo #{++i} ...");
                System.Console.WriteLine();
                InsertCompCsvToPg(file, schema);
                System.Console.WriteLine($"*** Archivo #{i} procesado...");
                System.Console.WriteLine();
            }
            System.Console.WriteLine($"*** Procesados en total {i} archivos!");            
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
        static void InsertAllTrepCsvToPg(string trepCsvPath, string schema = "raw_reports")
        {
            var files = from n in CsvFile.GetAllFilenames(trepCsvPath) orderby n 
                            where Path.GetExtension(n) == ".csv" select n;
            System.Console.WriteLine($"*** Insertando csv files from {trepCsvPath}");
            System.Console.WriteLine();
            int i = 0;
            foreach (var file in files)
            {
                System.Console.WriteLine($"*** Procesando archivo #{++i} ...");
                System.Console.WriteLine();
                InsertTrepCsvToPg(file, schema);
                System.Console.WriteLine($"*** Archivo #{i} procesado...");
            }            
            System.Console.WriteLine($"*** Procesados en total {i} archivos!");            
        }
        static void InsertCompCsvToPg(string compCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(compCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_comp_" + tableName;
            tableName = tableName.Replace('.', '_');
            PathWriter = $"/Volumes/easystore/Temp/{tableName}.txt";
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
        static void InsertTrepCsvToPg(string trepCsvName, string schema = "raw_reports")
        {
            var tableName = Path.GetFileNameWithoutExtension(trepCsvName);
            tableName = tableName.Replace("report_","");
            tableName = tableName.Replace("trep_","");
            tableName = "acta_trep_" + tableName;
            tableName = tableName.Replace('.', '_');
            PathWriter = $"/Volumes/easystore/Temp/{tableName}.txt";
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
        static void RenameFile(string oldPath, string newPath)
        {
            FileInfo fi = new FileInfo(oldPath);
            fi.MoveTo(newPath);
        }
        static void RenameFiles(string directory, string oldPrefix, string newPrefix)
        {
            IEnumerable<string> oldFilePaths = GetAllFilePathsBeginsWith(directory, oldPrefix);
            foreach (var oldPath in oldFilePaths)
            {
                string newPath = oldPath.Replace(oldPrefix, newPrefix);
                RenameFile(oldPath, newPath);
            }
        }
        static void WriteDiffFileNames(string path1, string path2, string extension, bool withoutExtension = false)
        {
            var filesDiff = DiffFilesNames(path1, path2, extension, withoutExtension);
            System.Console.WriteLine("Archivos Diferentes");
            System.Console.WriteLine("----------------------------------------------------------");
            int i = 1;
            foreach (var file in filesDiff)
            {
                System.Console.WriteLine($"{i++}) {file}");                                
            }

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
        static void Garbage()
        {
            // FilesExcelToCsv("/Volumes/easystore/trep_xls", "/Volumes/easystore/Csv");
            
            // WriteDiffFileNames("/Volumes/easystore/Dropbox/EReportes/Trep", "/Volumes/easystore/Csv", 
            //                                 "*.csv");

            // (int v, int t) = CsvFile.CountCsvTrepFiles("/Volumes/easystore/Csv");
            // System.Console.WriteLine($"{v}/{t}");

            // (int e, int t) = CsvFile.DeleteCsvTrepFilesInvalidos("/Volumes/easystore/Csv");
            // System.Console.WriteLine($"{e}/{t}");
            // InsertTrepCsvToPg("/Volumes/easystore/Dropbox/EReportes/Trep/trep_2019.10.20.11.34.57.csv");
            // CopyCsvCompToPg("/Volumes/easystore/Dropbox/EReportes/Computo/trep_2019.10.25.21.06.30.csv");
            // InsertCompCsvToPg("/Volumes/easystore/Dropbox/EReportes/Computo/trep_2019.10.25.21.06.30.csv");
            // InsertAllCompCsvToPg("/Users/luisosinaga/TEReportes/Comp");
            // InsertAllTrepCsvToPg("/Users/luisosinaga/TEReportes/Trep");

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

        #endregion
    }
}
