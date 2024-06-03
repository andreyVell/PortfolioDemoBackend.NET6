using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.AvetonRole;
using WebApi.Controllers.Base;
using WebApi.DTOs.AvetonRoles;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvetonRolesController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAvetonRoleService _service;

        public AvetonRolesController(IMapper mapper, IAvetonRoleService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(typeof(PageModel<GetAvetonRoleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _service.GetAllAsync();
                var response = new PageModel<GetAvetonRoleModel>()
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
        [ProducesResponseType(typeof(PageModel<GetAvetonRoleModel>), StatusCodes.Status200OK)]
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


        [HttpGet("{avetonRoleId}"), Authorize]
        [ProducesResponseType(typeof(GetAvetonRoleModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid avetonRoleId)
        {
            try
            {
                return Ok(await _service.GetAsync(avetonRoleId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAvetonRoleRequest newAvetonRole)
        {
            try
            {
                var avetonRole = _mapper.Map<CreateAvetonRoleModel>(newAvetonRole);
                return Ok(await _service.CreateAsync(avetonRole));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateAvetonRoleRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateAvetonRoleModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpDelete("{avetonRoleId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid avetonRoleId)
        {
            try
            {
                await _service.DeleteAsync(avetonRoleId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
