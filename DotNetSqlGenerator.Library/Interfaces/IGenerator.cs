using DotNetSqlGenerator.Library.Objects;

namespace DotNetSqlGenerator.Library.Interfaces
{
    public interface IGenerator
    {
        int RunNonQuery(string query);
        object RunScalar(string query);

        QueryInformation ExecuteInsert(Table T);
        QueryInformation ExecuteBulkInsert(Table T, int howMany);
        QueryInformation ExecuteDelete(Table T);
        QueryInformation ExecuteUpdate(Table T);
        QueryInformation ExecuteSelect(Table T, int columnsToReturn = -1, int columnsToSearch = -1);
    }
}
