using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.StageReportAttachedFile;
using WebApi.Controllers.Base;
using WebApi.DTOs.StageReportAttachedFile;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageReportAttachedFilesController : ApiControllerBase
    {
        private readonly IStageReportAttachedFileService _service;
        private readonly IMapper _mapper;

        public StageReportAttachedFilesController(IStageReportAttachedFileService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpDelete("{stageReportAttachedFileId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid stageReportAttachedFileId)
        {
            try
            {
                await _service.DeleteAsync(stageReportAttachedFileId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpGet("{stageReportAttachedFileId}/FileContent"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFileContentAsync([FromRoute] Guid stageReportAttachedFileId, [FromQuery] bool isImageMedium = false)
        {
            try
            {
                return Ok(await _service.GetFileContentAsync(stageReportAttachedFileId, isImageMedium));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStageReportAttachedFileRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateStageReportAttachedFileModel>(newRequest);
                return Ok(await _service.CreateAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
