using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KironTest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]    
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;

        public HolidayController(IHolidayService _holidayService)
        {
            this._holidayService = _holidayService;
        }

        [HttpGet("[action]")]
        [Route("InitializeUKBankHolidays")]
        public async Task<IActionResult> InitializeUKBankHolidays()
        {
            try
            {
                await _holidayService.InitializeUKBankHolidays();
                return Ok();
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Route("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
            try
            {                
                return Ok(await _holidayService.GetAllRegions());
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        [Route("GetAllBankHolidaysById")]
        public async Task<IActionResult> GetAllBankHolidaysById(int RegionId)
        {
            try
            {
                return Ok(await _holidayService.GetAllBankHolidaysById(RegionId));
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }
    }
}
