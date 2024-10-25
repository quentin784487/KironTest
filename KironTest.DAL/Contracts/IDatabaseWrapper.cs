using Dapper;
using System.Data;

namespace KironTest.DAL.Contracts
{
    public interface IDatabaseWrapper
    {
        IDbConnection GetConnection();
        IDbTransaction BeginTransaction(IDbConnection connection);
        void ReleaseConnection(IDbConnection connection);
        Task ExecuteStoredProcedureAsync(string storedProcedure, DynamicParameters parameters, IDbConnection connection, IDbTransaction transaction);
        Task<T> QuerySingleAsync<T>(string storedProcedure, DynamicParameters parameters, IDbConnection connection);
        Task<IEnumerable<T>> QueryListAsync<T>(string storedProcedure, IDbConnection connection, DynamicParameters? parameters = null);
    }
}
