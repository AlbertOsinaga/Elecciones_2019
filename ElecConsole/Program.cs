using System;
using System.IO;
using ElecLibrary;

namespace ElecConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine();
            Console.WriteLine("Programa 'Elecciones 2019'");
            System.Console.WriteLine();

            System.Console.WriteLine($"Inicio proceso: {DateTime.Now.ToString()}");
            System.Console.WriteLine();

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

            System.Console.WriteLine();
            System.Console.WriteLine($"Fin de proceso: {DateTime.Now.ToString()}");

            System.Console.WriteLine();
        }

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
            var dbRepo = new ElecRepository(dbPath);
            dbRepo.CreateSQLiteActasTable(tableName);
            var lines = CsvFile.ReadAllLines(repoCsvName, 2);
            System.Console.WriteLine($"Database: {dbPath}");
            System.Console.WriteLine($"Tabla: {tableName}");
            System.Console.WriteLine($"Insertando reporte: {repoCsvName}");
            System.Console.WriteLine("--------------------------------------------------------------");
            foreach (var linea in lines)
            {
                Acta acta = CsvFile.GetActa(linea, repoCsvName);
                long actaId = dbRepo.InsertActa(acta, tableName);
                System.Console.WriteLine($"Acta insertada: {actaId}");
            }
            System.Console.WriteLine();
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
    }
}
