using System.Collections.Generic;

namespace ElecLibrary
{
    public interface IElecRepository
    {
        Acta GetActa(int id);
        IEnumerable<Acta> GetActasXMesa(string numeroMesa);
        IEnumerable<Acta> GetActasXRecinto(string recinto);
        IEnumerable<Acta> GetAllActas();
        long InsertActa(Acta acta);
    }
}