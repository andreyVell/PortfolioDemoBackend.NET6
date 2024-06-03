using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Authentication;
using WebApi.Controllers.Base;
using WebApi.DTOs.Authentication;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public AuthenticationController(
            IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                var newUser = _mapper.Map<LoginUserModel>(request);
                var token = await _authenticationService.LoginAsync(newUser);
                return Ok(new LoginUserResponse { Token = token });
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
