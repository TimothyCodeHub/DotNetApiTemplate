using System.Collections.Generic;
using System.Linq;
using DotNetApiTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetApiTemplate.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<string, string> _exceptionMap;

        public ApiExceptionFilter()
        {
            // 將特定異常類型映射到友好的錯誤消息
            _exceptionMap = new Dictionary<string, string>
            {
                { "System.ArgumentException", "無效的參數值" },
                { "System.ArgumentNullException", "必須提供參數值" },
                { "System.InvalidOperationException", "無效的操作" },
                { "System.NotImplementedException", "功能尚未實現" },
                { "System.Data.DataException", "數據操作錯誤" }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType().FullName;
            var message = _exceptionMap.ContainsKey(exceptionType)
                ? _exceptionMap[exceptionType]
                : "發生了未處理的異常";

            var apiResponse = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status500InternalServerError
            };

#if DEBUG
            apiResponse.Errors = new
            {
                ExceptionType = exceptionType,
                ExceptionMessage = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            };
#endif

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
} 