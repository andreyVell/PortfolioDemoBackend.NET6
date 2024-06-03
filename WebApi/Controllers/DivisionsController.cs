using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Division;
using WebApi.Controllers.Base;
using WebApi.DTOs.Division;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDivisionService _service;

        public DivisionsController(
            IMapper mapper, 
            IDivisionService divisionService)
        {
            _mapper = mapper;
            _service = divisionService;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(PageModel<GetDivisionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _service.GetAllAsync();
                var response = new PageModel<GetDivisionModel>()
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
        [ProducesResponseType(typeof(PageModel<GetDivisionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                // api/JobDivision/Page?startIndex=0&itemsPerPage=50&filterString=test
                return Ok(await _service.GetPageAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpGet("{divisionId}"), Authorize]
        [ProducesResponseType(typeof(GetDivisionModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid divisionId)
        {
            try
            {
                return Ok(await _service.GetAsync(divisionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDivisionRequest newDivision)
        {
            try
            {
                var division = _mapper.Map<CreateDivisionModel>(newDivision);
                return Ok(await _service.CreateAsync(division));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpDelete("{divisionId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid divisionId)
        {
            try
            {
                await _service.DeleteAsync(divisionId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("ParentDivisions"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetDivisionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetParentDivisionsAsync()
        {
            try
            {
                return Ok(await _service.GetParentDivisionsAsync());
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{parentDivisionId}/ChildDivisions"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetDivisionModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildDivisionsAsync([FromRoute] Guid parentDivisionId)
        {
            try
            {
                return Ok(await _service.GetChildDivisionsAsync(parentDivisionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("NestedList"), Authorize]
        [ProducesResponseType(typeof(List<GetDivisionWithChildsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNestedListAsync([FromQuery] string? filterString = "")
        {
            try
            {
                return Ok(await _service.GetNestedListAsync(filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDivisionRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateDivisionModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

    }
}
