using KironTest.DataModel;

namespace KironTest.Service.Contracts
{
    public interface ICoinStatsService
    {
        Task<PagedResult> GetCoinStats();
    }
}
