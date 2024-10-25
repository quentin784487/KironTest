using KironTest.Caching.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;

namespace KironTest.Service
{
    public class CoinStatsService : ICoinStatsService
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ICoinStatsRepository _coinStatsRepository;

        public CoinStatsService(ICoinStatsRepository _coinStatsRepository, IMemoryCacheService _memoryCacheService)
        {
            this._coinStatsRepository = _coinStatsRepository;
            this._memoryCacheService = _memoryCacheService;
        }

        public async Task<PagedResult> GetCoinStats()        
        {
            try
            {
                var cachedData = _memoryCacheService.Get<PagedResult>("coin_stats_cache");
                if (cachedData != null)
                    return cachedData;

                var data = await _coinStatsRepository.GetCoinStats();

                _memoryCacheService.Set<PagedResult>("coin_stats_cache", data, TimeSpan.FromHours(1));

                return data;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }
    }
}
