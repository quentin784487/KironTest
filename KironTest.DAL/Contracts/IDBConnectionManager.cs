using System.Data;

namespace KironTest.DAL.Contracts
{
    public interface IDBConnectionManager
    {
        IDbConnection GetConnection();
        void ReleaseConnection(IDbConnection connection);
    }
}
