using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using Npgsql;
using Dapper;
using Z.Dapper.Plus;

namespace ElecLibrary
{
    public class ElecPgRepository : IElecRepository, IDisposable
    {
        #region Properties

        public string StrCnn { get; set; }
        public DbConnection Cnn { get; set; }
        public bool CnnIsOpen { get; set; }
        
        #endregion

        #region Ctor, Open, Close & Dispose

        public ElecPgRepository(string strcnn = null)
        {
            StrCnn = strcnn;
            Cnn = null;
            CnnIsOpen = false;
            OpenCnn();
        }
        public void CloseCnn()
        {
            if(Cnn != null && CnnIsOpen)
            {
                Cnn.Close();
                CnnIsOpen = false;
            }
        }
        public void Dispose()
        {
            CloseCnn();
            StrCnn = null;
            Cnn = null;                
        }
        public void OpenCnn()
        {
            if(Cnn == null)
            {
                GetDbConnection();
                Cnn.Open();
                CnnIsOpen = true;
            }
        }

        #endregion

        #region Methods
        public int CopyCsvCompToPg(string csvPath, string tableName, string schema = "raw_reports")
        {
            int ret = Cnn.Execute($@"COPY {schema}.{tableName}
                                    (pais,numero_departamento,departamento,provincia,
                                        numero_municipio,municipio,circunscripcion,localidad,recinto,
                                        numero_mesa,codigo_mesa,eleccion,inscritos,
                                        cc,fpv,mts,ucs,mas_ipsp,_21f,pdc,mnr,pan_bol,
                                        votos_validos,blancos,nulos, estado_acta) 
                                    FROM '{csvPath}' DELIMITER ',' CSV HEADER;");
            return ret;
        }
        public int CopyCsvTrepToPg(string csvPath, string tableName, string schema = "raw_reports")
        {
            int ret = Cnn.Execute($@"COPY {schema}.{tableName}
                                    (pais,numero_departamento,departamento,provincia,
                                        numero_municipio,municipio,circunscripcion,localidad,recinto,
                                        numero_mesa,codigo_mesa,eleccion,inscritos,
                                        cc,fpv,mts,ucs,mas_ipsp,_21f,pdc,mnr,pan_bol,
                                        votos_validos,blancos,nulos) 
                                    FROM '{csvPath}' DELIMITER ',' CSV HEADER;");
            return ret;
        }
        public void CreatePgActaCompTable(string tableName = "acta_comp", string schema = "raw_reports")
        {
            Cnn.Execute(
                $@"CREATE TABLE {schema}.{tableName}
                (
                    id                  serial NOT NULL,
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
                    nulos               integer,
                    estado_acta         text,
                    CONSTRAINT {tableName}_key PRIMARY KEY (id)
                )"
            );
        }
        public void CreatePgActaTrepTable(string tableName = "acta_trep", string schema = "raw_reports")
        {
            Cnn.Execute(
                $@"CREATE TABLE {schema}.{tableName}
                (
                    id                  serial NOT NULL,
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
                    nulos               integer,
                    CONSTRAINT {tableName}_key PRIMARY KEY (id)
                )"
            );
        }
        public DbConnection GetDbConnection()
        {
            Cnn = null;
            if(!string.IsNullOrEmpty(StrCnn))
                Cnn = new NpgsqlConnection(StrCnn);
            return Cnn;
        }
        public DapperPlusActionSet<PgActaComp> InsertActasComp(IEnumerable<PgActaComp> actas, string tableName, string schema = "raw_reports")
        {
            DapperPlusManager.Entity<PgActaComp>().Table($"{schema}.{tableName}");
            var result = Cnn.BulkInsert(actas);
            return result;
        }
        public DapperPlusActionSet<PgActaTrep> InsertActasTrep(IEnumerable<PgActaTrep> actas, string tableName, string schema = "raw_reports")
        {
            DapperPlusManager.Entity<PgActaTrep>().Table($"{schema}.{tableName}");
            var result = Cnn.BulkInsert(actas);
            return result;
        }
        public long InsertPgActaComp(PgActaComp acta, string tableName, string schema = "raw_reports")
        {
            if(acta == null)
                return 0;
            
            long ret = Cnn.Execute($@"INSERT INTO {schema}.{tableName} (pais,numero_departamento,departamento,provincia,
                                        numero_municipio,municipio,circunscripcion,localidad,recinto,
                                        numero_mesa,codigo_mesa,eleccion,inscritos,
                                        cc,fpv,mts,ucs,mas_ipsp,_21f,pdc,mnr,pan_bol,
                                        votos_validos,blancos,nulos, estado_acta) " + 
                                    $@"VALUES (@pais,@numero_departamento,@departamento,@provincia,
                                        @numero_municipio,@municipio,@circunscripcion,@localidad,@recinto,
                                        @numero_mesa,@codigo_mesa,@eleccion,@inscritos,
                                        @cc,@fpv,@mts,@ucs,@mas_ipsp,@_21f,@pdc,@mnr,@pan_bol,
                                        @votos_validos,@blancos,@nulos,@estado_acta)", acta);
            return ret;
        }
        public long InsertPgActaTrep(PgActaTrep acta, string tableName, string schema = "raw_reports")
        {
            if(acta == null)
                return 0;
            long ret = Cnn.Execute($@"INSERT INTO {schema}.{tableName} (pais,numero_departamento,departamento,provincia,
                                        numero_municipio,municipio,circunscripcion,localidad,recinto,
                                        numero_mesa,codigo_mesa,eleccion,inscritos,
                                        cc,fpv,mts,ucs,mas_ipsp,_21f,pdc,mnr,pan_bol,
                                        votos_validos,blancos,nulos) " + 
                                    $@"VALUES (@pais,@numero_departamento,@departamento,@provincia,
                                        @numero_municipio,@municipio,@circunscripcion,@localidad,@recinto,
                                        @numero_mesa,@codigo_mesa,@eleccion,@inscritos,
                                        @cc,@fpv,@mts,@ucs,@mas_ipsp,@_21f,@pdc,@mnr,@pan_bol,
                                        @votos_validos,@blancos,@nulos)", acta);
            return ret;
        }

        #endregion

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
            IEnumerable<Acta> query = Cnn.Query<Acta>($"SELECT * FROM {tableName}");
            return query;
        }

        public long InsertActa(Acta acta, string tableName = "Actas")
        {
            if(acta == null)
                return 0;
            
            long actaId = Cnn.Query<long>($@"INSERT INTO {tableName} (Pais,NumeroDepartamento,Departamento,Provincia,
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

        #endregion
    }
}