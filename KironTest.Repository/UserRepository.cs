using Dapper;
using KironTest.DAL.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KironTest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;
        private readonly IConfiguration _configuration;
        private readonly string? _maxConnections;

        public UserRepository(IDatabaseWrapper _databaseWrapper, IConfiguration _configuration)
        {
            this._databaseWrapper = _databaseWrapper;
            _maxConnections = _configuration.GetSection("MaxConnections").Value;
        }

        public async Task<int> AddUser(User user)
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    using (var transaction = _databaseWrapper.BeginTransaction(connection))
                    {
                        try
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("Username", user.Username);
                            parameters.Add("Password", user.Password);
                            parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                            await _databaseWrapper.ExecuteStoredProcedureAsync("[dbo].[sp_SetUser]", parameters, connection, transaction);
                            transaction.Commit();

                            return parameters.Get<int>("Id");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new DataAccessException("Error executing stored procedure '[dbo].[sp_SetUser]'", ex);
                        }
                        finally
                        {
                            _databaseWrapper.ReleaseConnection(connection);
                        }
                    }
                }                    
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }

        public async Task<User> GetUser(string userName)
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Username", userName);

                        return await _databaseWrapper.QuerySingleAsync<User>("[dbo].[sp_GetUser]", parameters, connection);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Error executing stored procedure '[dbo].[sp_GetUser]'", ex);
                    }
                    finally
                    {
                        _databaseWrapper.ReleaseConnection(connection);
                    }
                }                    
            }
            catch (MaximumConnectionsException ex)
            {
                throw new DataAccessException($"A maximum of '{_maxConnections}' are allowed.", ex);
            }
        }
    }
}
