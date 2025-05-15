using DotNetApiTemplate.Extensions;
using DotNetApiTemplate.Filters;
using DotNetApiTemplate.Utils;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 設置Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 添加服務到容器
// 數據庫上下文
builder.Services.AddApplicationDbContext(builder.Configuration);

// 儲存庫
builder.Services.AddRepositories();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// JWT認證
builder.Services.AddJwtAuthentication(builder.Configuration);

// 控制器與過濾器
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
    options.Filters.Add<ModelValidationFilter>();
})
.AddFluentValidation(fv => 
{
    fv.ImplicitlyValidateChildProperties = true;
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
});

// Swagger文檔
builder.Services.AddSwaggerDocumentation();

// 跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 註冊JWT工具
builder.Services.AddScoped<JwtTokenGenerator>();

// 註冊其他服務
// builder.Services.AddScoped<IUserService, UserService>();

// 構建應用程序
var app = builder.Build();

// 配置HTTP請求管道
// 全局異常處理
app.UseExceptionHandling();

// 配置Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetApiTemplate API v1"));
}

// API響應包裝（確保在異常處理之後）
app.UseApiResponseWrapper();

// 其他中間件
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting DotNetApiTemplate API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "DotNetApiTemplate API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
