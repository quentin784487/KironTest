using KironTest.Caching.Contracts;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using KironTest.Shared.ViewModels;
using Quartz;

namespace KironTest.Service
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ISchedulerFactory _schedulerFactory;

        public HolidayService(IHolidayRepository _holidayRepository, IMemoryCacheService _memoryCacheService, ISchedulerFactory _schedulerFactory)
        {
            this._holidayRepository = _holidayRepository;
            this._memoryCacheService = _memoryCacheService;
            this._schedulerFactory = _schedulerFactory;
        }

        public async Task InitializeUKBankHolidays()
        {
            try
            {
                if (await _holidayRepository.HasAny())
                    throw new ProcessFulfilledException();

                await SetUKBankHolidays();

                var scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.Start();
            }
            catch (ProcessFulfilledException ex)
            {
                throw new ServiceException("The work for this endpoint has been fulfilled.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Could not initialize process.", ex);
            }
        }        

        public async Task ProcessUKBankHolidays()
        {
            try
            {
                await SetUKBankHolidays();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        private async Task SetUKBankHolidays()
        {
            List<Region> regions = new List<Region>();
            Dictionary<string, Region> _regions = await _holidayRepository.GetUKBankHolidays();

            foreach (var _region in _regions)
            {
                Region region = new Region() { Name = _region.Key };
                region.Division = _region.Value.Division;

                foreach (var _event in _region.Value.Events)
                {
                    region.Events.Add(new Event() { Title = _event.Title, Date = _event.Date, Notes = _event.Notes == String.Empty ? null : _event.Notes, Bunting = _event.Bunting });
                }
                regions.Add(region);
            }
            await _holidayRepository.SetUKBankHolidays(regions);
        }

        public async Task<IEnumerable<RegionDTO>> GetAllRegions()
        {
            try
            {
                var cachedData = _memoryCacheService.Get<IEnumerable<RegionDTO>>("all_regions_cache");
                if (cachedData != null)
                    return cachedData;

                var data = await _holidayRepository.GetAllRegions();

                _memoryCacheService.Set<IEnumerable<RegionDTO>>("all_regions_cache", data, TimeSpan.FromMinutes(30));

                return data;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<BankHolidayDTO>> GetAllBankHolidaysById(int RegionId)
        {
            try
            {
                var cachedData = _memoryCacheService.Get<IEnumerable<BankHolidayDTO>>($"all_bank_holidays_cache_{RegionId}");
                if (cachedData != null)
                    return cachedData;

                var data = await _holidayRepository.GetAllBankHolidaysById(RegionId);

                _memoryCacheService.Set<IEnumerable<BankHolidayDTO>>($"all_bank_holidays_cache_{RegionId}", data, TimeSpan.FromMinutes(30));

                return data;
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message, ex);
            }
        }
    }
}
