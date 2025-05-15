using DotNetApiTemplate.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApiTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ApiResponseResult<T>(ApiResponse<T> response)
        {
            return StatusCode(response.StatusCode, response);
        }

        protected IActionResult Success<T>(T data, string? message = null)
        {
            return Ok(ApiResponse<T>.SuccessResponse(data, message));
        }

        protected IActionResult Created<T>(T data, string? message = null)
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            response.StatusCode = 201;
            return StatusCode(201, response);
        }

        protected IActionResult ApiNoContent(string? message = null)
        {
            var response = ApiResponse<object>.SuccessResponse(null, message);
            response.StatusCode = 204;
            return StatusCode(204, response);
        }

        protected IActionResult ResourceNotFound(string message = "Resource not found")
        {
            return base.NotFound(ApiResponse<object>.NotFoundResponse(message));
        }

        protected IActionResult ApiBadRequest(string message, object? errors = null)
        {
            return base.BadRequest(ApiResponse<object>.ErrorResponse(message, errors));
        }

        protected IActionResult ApiUnauthorized(string message = "Unauthorized")
        {
            return base.Unauthorized(ApiResponse<object>.UnauthorizedResponse(message));
        }

        protected IActionResult ServerError(string message = "Internal server error")
        {
            return StatusCode(500, ApiResponse<object>.ServerErrorResponse(message));
        }
    }
} 