using KironTest.Shared.ViewModels;

namespace KironTest.Service.Contracts
{
    public interface IHolidayService
    {
        Task InitializeUKBankHolidays();
        Task ProcessUKBankHolidays();
        Task<IEnumerable<RegionDTO>> GetAllRegions();
        Task<IEnumerable<BankHolidayDTO>> GetAllBankHolidaysById(int RegionId);
    }
}
