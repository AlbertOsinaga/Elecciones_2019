namespace ElecLibrary
{
    public class Acta
    {
        public int Id { get; set; } // Id
        public string Pais { get; set; }    // País
        public string NumeroDepartamento { get; set; }  // Número departamento
        public string Departamento { get; set; }    // Departamento
        public string Provincia { get; set; }   // Provincia
        public string NumeroMunicipio { get; set; } // Número municipio
        public string Municipio { get; set; }   // Municipio
        public string Circunscripcion { get; set; } // Circunscripción
        public string Localidad { get; set; }   // Localidad
        public string Recinto { get; set; } // Recinto
        public string NumeroMesa { get; set; }  // Número Mesa
        public string CodigoMesa { get; set; }  // Código Mesa
        public string Eleccion { get; set; }    // Elección
        public string Inscritos { get; set; }   // Inscritos
        public string CC { get; set; }  // CC
        public string FPV { get; set; } // FPV
        public string MTS { get; set; } // MTS
        public string UCS { get; set; } // UCS
        public string MAS_IPSP { get; set; }    // MAS - IPSP
        public string F21 { get; set; } // 21F
        public string PDC { get; set; } // PDC
        public string MNR { get; set; } // MNR
        public string PAN_BOL { get; set; } // PAN-BOL
        public string VotosValidos { get; set; }    // Votos Válidos
        public string Blancos { get; set; } // Blancos
        public string Nulos { get; set; }   // Nulos
        public string EstadoActa { get; set; }  // Estado acta
        public string Fecha { get; set; }   // Fecha
        public string Origen { get; set; }  // Origen
        public string TimeStamp { get; set; }   // TimeStamp
        public string Extras { get; set; }  // Extras
        public string Otros { get; set; }   // Otros

        public override string ToString()
        {
            return $"Id:{Id}\nPais: {Pais}\nNumeroDepartamento: {NumeroDepartamento}\nDepartamento: {Departamento}\nProvincia: {Provincia}" + 
                    $"\nNumeroMunicipio: {NumeroMunicipio}\nMunicipio: {Municipio}\nCircunscripcion: {Circunscripcion}\nLocalidad: {Localidad}\nRecinto: {Recinto}" +
                    $"\nNumeroMesa: {NumeroMesa}\nCodigoMesa: {CodigoMesa}\nEleccion: {Eleccion}\nInscritos: {Inscritos}\nCC: {CC}\nFPV: {FPV}\nMTS: {MTS}" +
                    $"\nUCS: {UCS}\nMAS_IPSP: {MAS_IPSP}\nF21: {F21}\nPDC: {PDC}\nMNR: {MNR}\nPAN_BOL: {PAN_BOL}\nVotosValidos: {VotosValidos}" +
                    $"\nBlancos: {Blancos}\nNulos: {Nulos}\nEstadoActa: {EstadoActa}\nFecha: {Fecha}\nOrigen: {Origen}\nTimeStamp: {TimeStamp}\nExtras: {Extras}\nOtros: {Otros}";
        }    
    }
}