using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemManagmentController : ApiControllerBase
    {
        private readonly ISystemManagmentService _systemManagmentService;
        public SystemManagmentController(ISystemManagmentService systemManagmentService)
        {
            _systemManagmentService = systemManagmentService;
        }


        [HttpPost("ActivateOrganization/{organizationId}")]
        public async Task<IActionResult> ActivateOrganizationAsync([FromRoute] Guid organizationId)
        {
            try
            {
                await _systemManagmentService.ActivateTrialAsync(organizationId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpPost("DeactivateOrganization/{organizationId}")]
        public async Task<IActionResult> DeactivateOrganizationAsync([FromRoute] Guid organizationId)
        {
            try
            {
                await _systemManagmentService.DeactivateTrialAsync(organizationId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
