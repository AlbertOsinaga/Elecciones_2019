namespace ElecLibrary
{
    public class PgActaComp
    {
        public string pais { get; set; }
        public int numero_departsmento { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public int numero_municipio { get; set; }
        public string municipio { get; set; }
        public string circunscripcion { get; set; }
        public string localidad { get; set; }
        public string recinto { get; set; }
        public int numero_mesa { get; set; }
        public long codigo_mesa { get; set; }
        public string eleccion { get; set; }
        public int inscritos { get; set; }
        public int cc { get; set; }
        public int fpv { get; set; }
        public int mts { get; set; }
        public int ucs { get; set; }
        public int mas_ipsp { get; set; }
        public int _21f { get; set; }
        public int pdc { get; set; }
        public int mnr { get; set; }
        public int pan_bol { get; set; }
        public int votos_validos { get; set; }
        public int blancos { get; set; }
        public int nulos { get; set; }
        public string estado_acta { get; set; }

        public override string ToString()
        {
            return $"pais: {pais}\nnumero departamento: {numero_departsmento}\ndepartamento: {departamento}\nprovincia: {provincia}" + 
                    $"\nnumero municipio: {numero_municipio}\nmunicipio: {municipio}\ncircunscripcion: {circunscripcion}\nlocalidad: {localidad}\nrecinto: {recinto}" +
                    $"\nnumero mesa: {numero_mesa}\ncodigo_mesa: {codigo_mesa}\neleccion: {eleccion}\ninscritos: {inscritos}\ncc: {cc}\nfpv: {fpv}\nmts: {mts}" +
                    $"\nucs: {ucs}\nmas_ipsp: {mas_ipsp}\n21f: {_21f}\npdc: {pdc}\nMNR: {mnr}\npan_bol: {pan_bol}\nvotos_validos: {votos_validos}" +
                    $"\nblancos: {blancos}\nnulos: {nulos}\nestado acta: {estado_acta}";
        }    
    }
}