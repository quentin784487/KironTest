using KironTest.DAL.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace KironTest.Service
{
    public class SitemapRepository : ISitemapRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseWrapper _databaseWrapper;
        private readonly string _maxConnections;

        public SitemapRepository(IDatabaseWrapper _databaseWrapper, IConfiguration _configuration)
        {
            this._databaseWrapper = _databaseWrapper;
            _maxConnections = _configuration.GetSection("MaxConnections").Value;
        }

        public async Task<IEnumerable<Navigation>> GetSitemap()
        {
            try
            {
                using (var connection = _databaseWrapper.GetConnection())
                {
                    try
                    {
                        return await _databaseWrapper.QueryListAsync<Navigation>("[dbo].[sp_GetSitemap]", connection);
                    }
                    catch (Exception ex)
                    {
                        throw new DataAccessException("Error executing stored procedure '[dbo].[sp_GetSitemap]'", ex);
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
