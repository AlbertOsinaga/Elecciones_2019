using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using Npgsql;
using Dapper;

namespace ElecLibrary
{
    public class ElecPgRepository : IElecRepository
    {
        public string StrCnn { get; set; }
        public ElecPgRepository(string strcnn = null)
        {
            StrCnn = strcnn;
        }

        public void CreatePgActaCompTable(string tableName = "PgActaComp", string schema = "raw_reports")
        {
            using (var cnn = GetDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                $@"CREATE TABLE {schema}.{tableName}
                (
                    pais                text,
                    numero_departsmento integer,
                    departamento        text,
                    provincia           text,
                    numero_municipio    integer,
                    municipio           text,
                    circunscripcion     text,
                    localidad           text,
                    recinto             text,
                    numero_mesa         integer,
                    codigo_mesa         bigint,
                    eleccion            text,
                    inscritos           integer,
                    cc                  integer,
                    fpv                 integer,
                    mts                 integer,
                    ucs                 integer,
                    mas_ipsp            integer,
                    _21f                integer,
                    pdc                 integer,
                    mnr                 integer,
                    pan_bol             integer,
                    votos_validos       integer,
                    blancos             integer,
                    nulos               integer,
                    estado_acta         text
                )");
            }
        }

        public void CreatePgActaTrepTable(string tableName = "PgActaTrep", string schema = "raw_reports")
        {
            using (var cnn = GetDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                $@"CREATE TABLE {schema}.{tableName}
                (
                    pais                text,
                    numero_departamento integer,
                    departamento        text,
                    provincia           text,
                    numero_municipio    integer,
                    municipio           text,
                    circunscripcion     text,
                    localidad           text,
                    recinto             text,
                    numero_mesa         integer,
                    codigo_mesa         bigint,
                    eleccion            text,
                    inscritos           integer,
                    cc                  integer,
                    fpv                 integer,
                    mts                 integer,
                    ucs                 integer,
                    mas_ipsp            integer,
                    _21f                integer,
                    pdc                 integer,
                    mnr                 integer,
                    pan_bol             integer,
                    votos_validos       integer,
                    blancos             integer,
                    nulos               integer
                )");
            }
        }

        public DbConnection GetDbConnection()
        {
            var cnn = new NpgsqlConnection(StrCnn);
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