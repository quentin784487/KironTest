using Dapper;
using KironTest.DAL.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KironTest.DAL
{
    public class DatabaseWrapper : IDatabaseWrapper
    {
        private readonly IConfiguration _configuration;
        private readonly IDBConnectionManager _DBConnectionManager;
        private readonly string _connectionString;
        private readonly string _maxConnections;

        public DatabaseWrapper(IConfiguration _configuration, IDBConnectionManager _DBConnectionManager)
        {
            this._configuration = _configuration;
            this._DBConnectionManager = _DBConnectionManager;
            _connectionString = _configuration.GetSection("DbConnection").Value;
            _maxConnections = _configuration.GetSection("MaxConnections").Value;
        }

        public IDbConnection GetConnection()
        {
            return _DBConnectionManager.GetConnection();
        }

        public IDbTransaction BeginTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction();
        }

        public void ReleaseConnection(IDbConnection connection)
        {
            _DBConnectionManager.ReleaseConnection(connection);
        }

        public async Task ExecuteStoredProcedureAsync(string storedProcedure, DynamicParameters parameters, IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync(storedProcedure, parameters, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> QuerySingleAsync<T>(string storedProcedure, DynamicParameters parameters, IDbConnection connection)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryListAsync<T>(string storedProcedure, IDbConnection connection, DynamicParameters? parameters = null)
        {
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
