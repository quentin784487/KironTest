using KironTest.Service.Contracts;
using Quartz;

namespace KironTest.Scheduler.CronJobs
{
    public class ScheduledJob : IJob
    {
        private readonly IHolidayService _holidayService;

        public ScheduledJob(IHolidayService _holidayService)
        {
            this._holidayService = _holidayService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _holidayService.ProcessUKBankHolidays();
        }
    }
}
