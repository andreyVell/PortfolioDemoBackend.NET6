using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ProjectStage;
using WebApi.Controllers.Base;
using WebApi.DTOs.ProjectStage;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectStagesController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectStageService _service;

        public ProjectStagesController(IMapper mapper, IProjectStageService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetProjectStageModel>), StatusCodes.Status200OK)]
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

        [HttpGet("GetAllForProject/{projectId}"), Authorize]
        [ProducesResponseType(typeof(List<GetProjectStageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForProjectAsync([FromRoute] Guid projectId, [FromQuery] string? filterString = "")
        {
            try
            {                
                return Ok(await _service.GetAllForProjectAsync(projectId, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProjectStageRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateProjectStageModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{projectStageId}"), Authorize]
        [ProducesResponseType(typeof(GetProjectStageModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid projectStageId)
        {
            try
            {
                return Ok(await _service.GetAsync(projectStageId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectStageRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateProjectStageModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{projectStageId}/GetProjectName"), Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectNameAsync([FromRoute] Guid projectStageId)
        {
            try
            {
                return Ok(new { Name = await _service.GetProjectNameAsync(projectStageId) });
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{projectStageId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid projectStageId)
        {
            try
            {
                await _service.DeleteAsync(projectStageId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
