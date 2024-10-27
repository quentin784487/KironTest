using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KironTest.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoinStatsController : ControllerBase
    {
        private readonly ICoinStatsService _coinStatsService;

        public CoinStatsController(ICoinStatsService _coinStatsService, ILogger _logger)
        {
            this._coinStatsService = _coinStatsService;
        }

        [HttpGet("[action]")]
        [Route("GetCoinStats")]
        public async Task<IActionResult> GetCoinStats()
        {
            try
            {
                return Ok(await _coinStatsService.GetCoinStats());
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }
    }
}
