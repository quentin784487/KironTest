using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KironTest.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SitemapController : ControllerBase
    {
        private readonly ISitemapService _sitemapService;

        public SitemapController(ISitemapService _sitemapService)
        {
            this._sitemapService = _sitemapService;
        }
        
        [HttpPost("[action]")]
        [Route("GetSitemap")]
        public async Task<IActionResult> GetSitemap()
        {
            try
            {
                return Ok(await _sitemapService.GetSitemap());
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }            
        }
    }
}
