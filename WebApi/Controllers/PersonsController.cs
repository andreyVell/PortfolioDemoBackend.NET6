using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Person;
using WebApi.Controllers.Base;
using WebApi.DTOs.Person;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _service;

        public PersonsController(IMapper mapper, IPersonService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetPersonModel>), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> CreateAsync([FromBody] CreatePersonRequest newPerson)
        {
            try
            {
                var person = _mapper.Map<CreatePersonModel>(newPerson);
                return Ok(await _service.CreateAsync(person));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{personId}"), Authorize]
        [ProducesResponseType(typeof(GetPersonModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid personId)
        {
            try
            {
                return Ok(await _service.GetAsync(personId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdatePersonRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdatePersonModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{personId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid personId)
        {
            try
            {
                await _service.DeleteAsync(personId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
