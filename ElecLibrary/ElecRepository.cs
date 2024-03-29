using System.Security.Cryptography;
using System;
using System.Collections.Generic;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace ElecLibrary
{
    public class ElecRepository : IElecRepository
    {
        public string DbFileName { get; set; }
        public ElecRepository(string dbFileName = null)
        {
            DbFileName = dbFileName;
            if(DbFileName != null)
                CreateSQLiteDB(DbFileName);
        }

        public void CreateSQLiteDB(string dbFileName)
        {
            DbFileName = dbFileName;
            using(var cnn = GetDbConnection())
            {
                cnn.Open();
            } 
        }

        public void CreateSQLiteActasTable(string tableName = "Actas")
        {
            using (var cnn = GetDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                $@"CREATE TABLE IF NOT EXISTS {tableName}
                (
                    Id                  integer primary key AUTOINCREMENT,
                    Pais                varchar(100),
                    NumeroDepartamento  varchar(100),
                    Departamento        varchar(100),
                    Provincia           varchar(100),
                    NumeroMunicipio     varchar(100),
                    Municipio           varchar(100),
                    Circunscripcion     varchar(100),
                    Localidad           varchar(100),
                    Recinto             varchar(100),
                    NumeroMesa          varchar(100),
                    CodigoMesa          varchar(100),
                    Eleccion            varchar(100),
                    Inscritos           varchar(100),
                    CC                  varchar(100),
                    FPV                 varchar(100),
                    MTS                 varchar(100),
                    UCS                 varchar(100),
                    MAS_IPSP            varchar(100),
                    F21                 varchar(100),
                    PDC                 varchar(100),
                    MNR                 varchar(100),
                    PAN_BOL             varchar(100),
                    VotosValidos        varchar(100),
                    Blancos             varchar(100),
                    Nulos               varchar(100),
                    EstadoActa          varchar(100),
                    Fecha               varchar(100),
                    Origen              varchar(100),
                    TimeStamp           varchar(100),
                    Extras              varchar(250),
                    Otros               varchar(250) 
                )");
            }
        }

        public DbConnection GetDbConnection()
        {
            var cnn = new SQLiteConnection($"DataSource={DbFileName}");
            return cnn;
        }

        #region IElecRepository

        public Acta GetActa(int id, string tableName = "Actas")
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Acta> GetActasXMesa(string numeroMesa, string tableName = "Actas")
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Acta> GetActasXRecinto(string recinto, string tableName = "Actas")
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Acta> GetAllActas(string tableName = "Actas")
        {
            using(var cnn = GetDbConnection())
            {
                cnn.Open();
                IEnumerable<Acta> query = cnn.Query<Acta>($"SELECT * FROM {tableName}");
                return query;
            }
        }

        public long InsertActa(Acta acta, string tableName = "Actas")
        {
            if(acta == null)
                return 0;
            using(var cnn = GetDbConnection())
            {
                cnn.Open();
                long actaId = cnn.Query<long>($@"INSERT INTO {tableName} (Pais,NumeroDepartamento,Departamento,Provincia,
                                                NumeroMunicipio,Municipio,Circunscripcion,Localidad,Recinto,
                                                NumeroMesa,CodigoMesa,Eleccion,Inscritos,
                                                CC,FPV,MTS,UCS,MAS_IPSP,F21,PDC,MNR,PAN_BOL,
                                                VotosValidos,Blancos,Nulos,EstadoActa,
                                                Fecha,Origen,TimeStamp,Extras,Otros) " + 
                                        $@"VALUES (@Pais,@NumeroDepartamento,@Departamento,@Provincia,
                                                @NumeroMunicipio,@Municipio,@Circunscripcion,@Localidad,@Recinto,
                                                @NumeroMesa,@CodigoMesa,@Eleccion,@Inscritos,
                                                @CC,@FPV,@MTS,@UCS,@MAS_IPSP,@F21,@PDC,@MNR,@PAN_BOL,
                                                @VotosValidos,@Blancos,@Nulos,@EstadoActa,
                                                @Fecha,@Origen,@TimeStamp,@Extras,@Otros)" + 
                                        $@";SELECT last_insert_rowid()", acta).First();
                return actaId;
            }
        }

        #endregion
    }
}