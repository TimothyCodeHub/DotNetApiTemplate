using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetApiTemplate.Models.Common;
using Microsoft.AspNetCore.Http;

namespace DotNetApiTemplate.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 如果請求是 Swagger 或 SignalR 相關的，則跳過包裝
            if (context.Request.Path.StartsWithSegments("/swagger") || 
                context.Request.Path.StartsWithSegments("/hubs"))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            try
            {
                // 創建一個新的 MemoryStream 來捕獲響應
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // 調用下一個中間件
                await _next(context);

                // 如果響應狀態碼不在成功範圍內，不進行包裝
                if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
                {
                    await CopyToOriginalBodyStreamAsync(responseBody, originalBodyStream);
                    return;
                }

                // 檢查響應內容類型是否為 JSON
                if (!context.Response.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) ?? true)
                {
                    await CopyToOriginalBodyStreamAsync(responseBody, originalBodyStream);
                    return;
                }

                // 讀取響應內容
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

                // 如果響應內容為空，則跳過包裝
                if (string.IsNullOrEmpty(responseContent))
                {
                    await CopyToOriginalBodyStreamAsync(responseBody, originalBodyStream);
                    return;
                }

                // 檢查響應是否已經被包裝
                try
                {
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    if (jsonDoc.RootElement.TryGetProperty("success", out _) &&
                        jsonDoc.RootElement.TryGetProperty("statusCode", out _))
                    {
                        // 已經是一個 ApiResponse，不需要包裝
                        await CopyToOriginalBodyStreamAsync(responseBody, originalBodyStream);
                        return;
                    }
                }
                catch
                {
                    // 如果解析失敗，繼續進行包裝
                }

                // 包裝響應
                var wrappedResponse = new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = context.Response.StatusCode,
                    Data = JsonSerializer.Deserialize<object>(responseContent)
                };

                // 將包裝後的響應寫回
                context.Response.Body = originalBodyStream;
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(wrappedResponse, jsonOptions));
            }
            finally
            {
                // 確保響應主體被還原
                context.Response.Body = originalBodyStream;
            }
        }

        private static async Task CopyToOriginalBodyStreamAsync(MemoryStream memoryStream, Stream originalBodyStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }
} 