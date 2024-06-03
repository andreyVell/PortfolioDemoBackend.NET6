using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.AvetonRole;
using Services.Models.AvetonUser;
using WebApi.Controllers.Base;
using WebApi.DTOs.AvetonUser;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvetonUsersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAvetonUserService _service;

        public AvetonUsersController(IMapper mapper, IAvetonUserService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAvetonUserRequest newAvetonUser)
        {
            try
            {
                var avetonUser = _mapper.Map<CreateAvetonUserModel>(newAvetonUser);
                return Ok(await _service.CreateAsync(avetonUser));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateAvetonUserRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateAvetonUserModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpDelete("{avetonUserId}/DeleteRole/{roleId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] Guid? roleId, [FromRoute] Guid? avetonUserId)
        {
            try
            {
                await _service.DeleteRoleAsync(roleId, avetonUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }


        [HttpGet("{avetonUserId}/GetAllRoles"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetAvetonRoleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync([FromRoute] Guid? avetonUserId)
        {
            try
            {
                var items = await _service.GetAllRolesAsync(avetonUserId);
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




        [HttpPost("{avetonUserId}/AddRole/{roleId}"), Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid? roleId, [FromRoute] Guid? avetonUserId)
        {
            try
            {
                await _service.AddRoleToUserAsync(roleId, avetonUserId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
