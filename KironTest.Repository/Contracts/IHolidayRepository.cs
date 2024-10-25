using KironTest.DataModel;
using KironTest.Shared.ViewModels;

namespace KironTest.Repository.Contracts
{
    public interface IHolidayRepository
    {
        Task<Dictionary<string, Region>> GetUKBankHolidays();
        Task SetUKBankHolidays(List<Region> regions);
        Task<IEnumerable<RegionDTO>> GetAllRegions();
        Task<IEnumerable<BankHolidayDTO>> GetAllBankHolidaysById(int RegionId);
        Task<bool> HasAny();
    }
}
