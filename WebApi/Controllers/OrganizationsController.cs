using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Organization;
using WebApi.Controllers.Base;
using WebApi.DTOs.Organization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationService _service;

        public OrganizationsController(IMapper mapper, IOrganizationService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetOrganizationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                //Page?startIndex=0&itemsPerPage=50&filterString=test
                return Ok(await _service.GetPageAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrganizationRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateOrganizationModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{organizationId}"), Authorize]
        [ProducesResponseType(typeof(GetOrganizationModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid organizationId)
        {
            try
            {
                return Ok(await _service.GetAsync(organizationId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOrganizationRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateOrganizationModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{organizationId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid organizationId)
        {
            try
            {
                await _service.DeleteAsync(organizationId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
