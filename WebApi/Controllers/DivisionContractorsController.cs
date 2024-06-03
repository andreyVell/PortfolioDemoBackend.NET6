using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.DivisionContractor;
using WebApi.Controllers.Base;
using WebApi.DTOs.DivisionContractor;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionContractorsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDivisionContractorService _service;

        public DivisionContractorsController(IMapper mapper, IDivisionContractorService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDivisionContractorRequest newDivisionContractor)
        {
            try
            {
                var divisionContractor = _mapper.Map<CreateDivisionContractorModel>(newDivisionContractor);
                return Ok(await _service.CreateAsync(divisionContractor));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpDelete("{divisionContractorId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid divisionContractorId)
        {
            try
            {
                await _service.DeleteAsync(divisionContractorId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
