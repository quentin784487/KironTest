using AutoMapper;
using KironTest.DataModel;
using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using KironTest.Shared.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KironTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;

        public SecurityController(ISecurityService _securityService, IMapper _mapper)
        {
            this._securityService = _securityService;
            this._mapper = _mapper;
        }

        [HttpPost("[action]")]
        [Route("Authorize")]
        public async Task<IActionResult> Authorize([FromBody] UserDTO user)
        {
            try
            {
                var _userEntity = _mapper.Map<User>(user);
                var securityToken = await _securityService.Authorize(_userEntity);
                return Ok(securityToken);
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO user)
        {
            try
            {
                var _userEntity = _mapper.Map<User>(user);
                return Ok(await _securityService.Register(_userEntity));
            }
            catch (ServiceException ex)
            {
                //Log stack trace before returning message

                return StatusCode(500, ex.Message);
            }
        }
    }
}
