using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetApiTemplate.Controllers
{
    public class DemoController : BaseApiController
    {
        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Demo API called");
            return Success(new { message = "API模板工作正常！" });
        }

        [HttpGet("error")]
        public IActionResult GetError()
        {
            _logger.LogWarning("Demo error API called");
            return ApiBadRequest("這是一個示例錯誤", new { errorDetails = "這是一個示例錯誤詳情" });
        }

        [HttpGet("exception")]
        public IActionResult GetException()
        {
            _logger.LogWarning("Demo exception API called");
            throw new InvalidOperationException("這是一個示例異常");
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecure()
        {
            _logger.LogInformation("Secure API called");
            return Success(new { message = "這是一個受保護的API端點" });
        }

        [HttpGet("items")]
        public IActionResult GetItems([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("GetItems API called with pageIndex {PageIndex} and pageSize {PageSize}", pageIndex, pageSize);
            
            // 模擬分頁數據
            var items = new List<object>();
            for (int i = 1; i <= pageSize; i++)
            {
                items.Add(new { id = Guid.NewGuid(), name = $"Item {i + (pageIndex - 1) * pageSize}" });
            }
            
            return Success(new 
            { 
                items,
                pageIndex,
                pageSize,
                totalCount = 100,
                totalPages = 10
            });
        }
    }
} 