using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.StageReport;
using WebApi.Controllers.Base;
using WebApi.DTOs.StageReport;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageReportsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStageReportService _service;

        public StageReportsController(IMapper mapper, IStageReportService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStageReportRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateStageReportModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{stageReportId}"), Authorize]
        [ProducesResponseType(typeof(GetStageReportModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid stageReportId)
        {
            try
            {
                return Ok(await _service.GetAsync(stageReportId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{stageReportId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid stageReportId)
        {
            try
            {
                await _service.DeleteAsync(stageReportId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateStageReportRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateStageReportModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
