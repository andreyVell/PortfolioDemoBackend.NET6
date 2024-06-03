using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Position;
using WebApi.Controllers.Base;
using WebApi.DTOs.Position;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPositionService _service;

        public PositionsController(IMapper mapper, IPositionService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(PageModel<GetPositionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _service.GetAllAsync();
                var response = new PageModel<GetPositionModel>()
                {
                    Items = items,
                    TotalItems = items.Count,
                    StartIndex = 0,
                    ItemsPerPage = items.Count
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetPositionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                return Ok(await _service.GetPageAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpGet("{positionId}"), Authorize]
        [ProducesResponseType(typeof(GetPositionModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid positionId)
        {
            try
            {
                return Ok(await _service.GetAsync(positionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePositionRequest newPosition)
        {
            try
            {
                var position = _mapper.Map<CreatePositionModel>(newPosition);
                return Ok(await _service.CreateAsync(position));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpDelete("{positionId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid positionId)
        {
            try
            {
                await _service.DeleteAsync(positionId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdatePositionRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdatePositionModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
