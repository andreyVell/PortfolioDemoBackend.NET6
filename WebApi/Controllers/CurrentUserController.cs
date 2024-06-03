using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.AvetonRoleAccess;
using Services.Models.Employee;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentUserController : ApiControllerBase
    {
        private readonly ICurrentUserDataService _currentUserDataService;
        private readonly ICurrentUserEmployeeService _currentUserEmployeeService;
        private readonly IEmployeeService _employeeService;

        public CurrentUserController(
            ICurrentUserDataService currentUserDataService, 
            ICurrentUserEmployeeService currentUserEmployeeService,
            IEmployeeService employeeService)
        {
            _currentUserDataService = currentUserDataService;
            _currentUserEmployeeService = currentUserEmployeeService;
            _employeeService = employeeService;
        }

        [HttpGet("Employee"), Authorize]
        [ProducesResponseType(typeof(GetEmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeAsync()
        {
            try
            {                
                return Ok(await _currentUserEmployeeService.GetEmployeeForCurrentUserAsync());
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("EmployeeShortInfo"), Authorize]
        [ProducesResponseType(typeof(GetEmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeShortInfoAsync()
        {
            try
            {
                return Ok(await _currentUserEmployeeService.GetEmployeeShortInfoForCurrentUserAsync());
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("GetAccesses"), Authorize]
        [ProducesResponseType(typeof(List<GetAvetonRoleAccessModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccessesForCurrentUserAsync(
            [FromBody] string[] entityNames)
        {
            try
            {                
                return Ok(await _currentUserDataService.GetAccessesForCurrentUserAsync(entityNames));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
