using System.Net;

namespace DotNetApiTemplate.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public object? Errors { get; set; }

        public ApiResponse()
        {
            Success = true;
            StatusCode = (int)HttpStatusCode.OK;
        }

        public ApiResponse(T? data, string? message = null)
        {
            Success = true;
            Message = message;
            Data = data;
            StatusCode = (int)HttpStatusCode.OK;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public ApiResponse(string message, object? errors)
        {
            Success = false;
            Message = message;
            Errors = errors;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string? message = null)
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> ErrorResponse(string message, object? errors = null)
        {
            return new ApiResponse<T>(message, errors)
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        public static ApiResponse<T> NotFoundResponse(string message = "Resource not found")
        {
            return new ApiResponse<T>(message)
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        public static ApiResponse<T> UnauthorizedResponse(string message = "Unauthorized")
        {
            return new ApiResponse<T>(message)
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        public static ApiResponse<T> ServerErrorResponse(string message = "Internal server error")
        {
            return new ApiResponse<T>(message)
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 