using Microsoft.AspNetCore.Mvc;

namespace KironTest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Api has started.");
        }
    }
}
