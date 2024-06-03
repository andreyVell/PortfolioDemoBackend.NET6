using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.StageManager;
using WebApi.Controllers.Base;
using WebApi.DTOs.StageManager;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageManagersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStageManagerService _service;

        public StageManagersController(IMapper mapper, IStageManagerService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStageManagerRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateStageManagerModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpDelete("{stageManagerId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid stageManagerId)
        {
            try
            {
                await _service.DeleteAsync(stageManagerId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
