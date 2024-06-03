using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Client;
using WebApi.Controllers.Base;
using WebApi.DTOs.Client;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientService _service;

        public ClientsController(IMapper mapper, IClientService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetClientModel>), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateClientRequest newClient)
        {
            try
            {
                var client = _mapper.Map<CreateClientModel>(newClient);
                return Ok(await _service.CreateAsync(client));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{clientId}"), Authorize]
        [ProducesResponseType(typeof(GetClientModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid clientId)
        {
            try
            {
                return Ok(await _service.GetAsync(clientId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateClientRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateClientModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{clientId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid clientId)
        {
            try
            {
                await _service.DeleteAsync(clientId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
