using KironTest.DataModel;

namespace KironTest.Repository.Contracts
{
    public interface ICoinStatsRepository
    {
        Task<PagedResult> GetCoinStats();
    }
}
