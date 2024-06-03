using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Employee;
using WebApi.Controllers.Base;
using WebApi.DTOs.Employees;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _service;

        public EmployeesController(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _service = employeeService;
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetEmployeeModel>), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateEmployeeRequest newEmployee)
        {
            try
            {
                var employee = _mapper.Map<CreateEmployeeModel>(newEmployee);
                return Ok(await _service.CreateAsync(employee));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{employeeId}"), Authorize]
        [ProducesResponseType(typeof(GetEmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid employeeId)
        {
            try
            {
                return Ok(await _service.GetAsync(employeeId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{employeeId}/EmployeeSmallAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeSmallAvatarAsync([FromRoute] Guid employeeId)
        {
            try
            {
                return Ok(await _service.GetEmployeeSmallAvatarAsync(employeeId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{employeeId}/EmployeeBigAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeBigAvatarAsync([FromRoute] Guid employeeId)
        {
            try
            {
                return Ok(await _service.GetEmployeeBigAvatarAsync(employeeId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateEmployeeRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateEmployeeModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{employeeId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid employeeId)
        {
            try
            {
                await _service.DeleteAsync(employeeId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
