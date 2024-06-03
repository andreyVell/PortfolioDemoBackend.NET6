using DataCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.DTOs;

namespace WebApi.Controllers.Base
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult ExceptionResult(Exception exception)
        {
            var error = new ApiErrorResponse();
            if (exception is ActionNotAllowedException)
            {                
                error.ErrorMessage = exception.Message;
                return StatusCode((int)HttpStatusCode.Forbidden, error);
            }
            if (exception is EntityNotFoundException)
            {
                error.ErrorMessage = exception.Message;
                return StatusCode((int)HttpStatusCode.NotFound, error);
            }
            if (exception is ClientAlreadyExistsException)
            {
                error.ErrorMessage = exception.Message;
                return StatusCode((int)HttpStatusCode.Conflict, error);
            }
            error.ErrorMessage = exception.Message;
            return StatusCode((int)HttpStatusCode.BadRequest, error);
        }
    }
}
