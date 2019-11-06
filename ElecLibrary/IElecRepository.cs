using System.Collections.Generic;

namespace ElecLibrary
{
    public interface IElecRepository
    {
        Acta GetActa(int id, string tableName);
        IEnumerable<Acta> GetActasXMesa(string numeroMesa, string tableName);
        IEnumerable<Acta> GetActasXRecinto(string recinto, string tableName);
        IEnumerable<Acta> GetAllActas(string tableName);
        long InsertActa(Acta acta, string tableName);
    }
}