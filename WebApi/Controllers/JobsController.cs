using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Job;
using WebApi.Controllers.Base;
using WebApi.DTOs.Job;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJobService _service;

        public JobsController(IMapper mapper, IJobService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateJobRequest newJob)
        {
            try
            {
                var job = _mapper.Map<CreateJobModel>(newJob);
                return Ok(await _service.CreateAsync(job));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateJobRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateJobModel>(request);
                return Ok(await _service.UpdateAsync(updateModel));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
