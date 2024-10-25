using Kirontest.HttpClient.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.Extensions.Configuration;

namespace KironTest.Repository
{
    public class CoinStatsRepository : ICoinStatsRepository
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IConfiguration _configuration;

        public CoinStatsRepository(IHttpClientWrapper _httpClientWrapper, IConfiguration _configuration)
        {
            this._httpClientWrapper = _httpClientWrapper;
            this._configuration = _configuration;
        }

        public async Task<PagedResult> GetCoinStats()
        {
            try
            {
                return await _httpClientWrapper.GetAsync<PagedResult>(_configuration["CoinStats:Url"], _configuration["CoinStats:Key"], _configuration["CoinStats:ApiValue"]);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error fetching data from '{_configuration["CoinStats:Url"]}'", ex);
            }            
        }
    }
}
