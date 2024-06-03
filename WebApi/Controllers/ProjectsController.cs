using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Project;
using WebApi.Controllers.Base;
using WebApi.DTOs.Project;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _service;

        public ProjectsController(IMapper mapper, IProjectService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetProjectModel>), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateProjectRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateProjectModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{projectId}"), Authorize]
        [ProducesResponseType(typeof(GetProjectModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid projectId)
        {
            try
            {
                return Ok(await _service.GetAsync(projectId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateProjectModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{projectId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid projectId)
        {
            try
            {
                await _service.DeleteAsync(projectId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
