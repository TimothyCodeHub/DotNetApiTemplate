using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetApiTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetApiTemplate.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request.",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            // 根據異常類型設置不同的狀態碼和消息
            if (exception is ArgumentException || exception is ArgumentNullException)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = exception.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = "Unauthorized access";
            }
            else if (exception is KeyNotFoundException)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = "The requested resource was not found";
            }
            else
            {
                // 在生產環境中，不要暴露詳細的錯誤信息給客戶端
                #if DEBUG
                response.Message = exception.Message;
                response.Errors = new { StackTrace = exception.StackTrace };
                #endif
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
} 