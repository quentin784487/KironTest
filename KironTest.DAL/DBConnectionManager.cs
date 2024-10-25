using KironTest.DAL.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace KironTest.DAL
{
    public class DBConnectionManager : IDBConnectionManager
    {
        private readonly string _connectionString;
        private readonly ConcurrentBag<IDbConnection> _connections;
        private int _maxConnections;
        private int _currentConnections;

        public DBConnectionManager(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetSection("DbConnection").Value;
            _maxConnections = Convert.ToInt16(_configuration.GetSection("MaxConnections").Value);
            _connections = new ConcurrentBag<IDbConnection>();
        }

        public IDbConnection GetConnection()
        {
            if (_currentConnections >= _maxConnections)
                throw new MaximumConnectionsException();

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            _connections.Add(connection);
            _currentConnections++;

            return connection;
        }

        public void ReleaseConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
                _connections.TryTake(out _);
                _currentConnections--;
            }
        }
    }
}
