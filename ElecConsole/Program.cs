﻿using System;
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

            // var fields = CsvFile.ReadHeader("/Volumes/easystore/Dropbox/EReportes/Example.csv");
            // System.Console.WriteLine(fields);

            var dbRepo = new ElecRepository("/Volumes/easystore/Dropbox/EReportes/elec.db");
            dbRepo.CreateSQLiteActasTable();

            // var fileNames = CsvFile.GetAllFilenames("/Volumes/easystore/Dropbox/EReportes", "db");
            
            // foreach (var file in fileNames)
            // {
            //     System.Console.WriteLine(file);
            // }

            // var lines = CsvFile.ReadAllLines("/Volumes/easystore/Dropbox/EReportes/Example.csv", 2, 3);
            // foreach (var linea in lines)
            // {
            //     Acta acta = CsvFile.GetActa(linea);
            //     System.Console.WriteLine(acta);
            //     System.Console.WriteLine();
            // }

            var lines = CsvFile.ReadAllLines("/Volumes/easystore/Dropbox/EReportes/Example.csv", 2, 3);
            foreach (var linea in lines)
            {
                Acta acta = CsvFile.GetActa(linea);
                int actaId = dbRepo.InsertActa(acta);
                System.Console.WriteLine($"Acta insertada: {actaId}");
            }
            System.Console.WriteLine();

            var actas = dbRepo.GetAllActas();
            if(actas != null)
            {        
                foreach (var acta in actas)
                {
                    System.Console.WriteLine(acta);
                }
            }
            else
            {
                System.Console.WriteLine("No se encontraron actas en la db!");
            }

            System.Console.WriteLine();
        }
    }
}