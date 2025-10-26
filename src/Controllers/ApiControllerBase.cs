using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Dtos.Exceptions;
using StudentActivities.src.Exceptions;


namespace StudentActivities.src.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ILogger _logger;
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult ReturnException(Exception ex)
        {
            if (ex is Result userEx)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ExceptionBody
                {
                    Message = userEx.Message
                });
            }

            if (ex is KeyNotFoundException)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ExceptionBody
                {
                    Message = ex.Message
                });
            }

            if (ex is UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new ExceptionBody
                {
                    Message = ex.Message
                });
            }
            
            if (ex is ArgumentException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ExceptionBody
                {
                    Message = ex.Message
                });
            }

            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ExceptionBody
            {
                Message = "An unexpected error occurred."
            });
        }
    }
}