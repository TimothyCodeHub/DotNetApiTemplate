# .NET Core API 通用模板

這是一個用於快速開發 .NET Core Web API 的通用模板，內置了許多開箱即用的功能和最佳實踐。

## 功能特點

1. **全局異常處理**
   - 自動捕獲並處理API中的異常
   - 根據異常類型返回適當的HTTP狀態碼
   - 在開發環境中提供詳細的錯誤信息

2. **API響應標準化**
   - 統一的API響應格式
   - 自動包裝所有API響應為標準格式
   - 包含狀態碼、成功標誌、數據和錯誤信息

3. **JWT身份驗證**
   - 內置JWT令牌生成和驗證
   - 支持角色和權限

4. **模型驗證**
   - 使用FluentValidation進行請求模型驗證
   - 自動返回友好的驗證錯誤信息

5. **日誌記錄**
   - 使用Serilog進行結構化日誌記錄
   - 同時輸出到控制台和文件
   - 支持日誌滾動和保留策略

6. **泛型儲存庫模式**
   - 實現統一的數據訪問方式
   - 支持異步操作

7. **Swagger API文檔**
   - 自動生成API文檔
   - 支持JWT令牌授權

8. **跨域支持**
   - 開箱即用的CORS配置

## 技術棧

- .NET 7.0
- Entity Framework Core 7.0
- Serilog
- FluentValidation
- AutoMapper
- Swagger/OpenAPI
- JWT認證

## 如何使用

### 前提條件

- .NET 7.0 SDK 或更高版本
- 支持的IDE（Visual Studio, VS Code, Rider等）

### 快速開始

1. **克隆項目**

```bash
git clone https://your-repository/DotNetApiTemplate.git
cd DotNetApiTemplate
```

2. **還原依賴項**

```bash
dotnet restore
```

3. **配置連接字符串**

在 `appsettings.json` 中修改 `ConnectionStrings:DefaultConnection` 為您的數據庫連接字符串。

4. **運行遷移（可選）**

```bash
dotnet ef database update
```

5. **運行項目**

```bash
dotnet run
```

6. **訪問Swagger UI**

打開瀏覽器訪問 `https://localhost:5001/swagger`

### 添加新的API控制器

1. 創建新的控制器類，繼承 `BaseApiController`：

```csharp
public class ProductsController : BaseApiController
{
    // 實現您的API端點
}
```

2. 使用內置的響應方法返回標準化響應：

```csharp
// 返回成功響應
return Success(data);

// 返回錯誤響應
return BadRequest("錯誤信息");

// 返回未找到響應
return NotFound("資源未找到");
```

### 添加新的實體和儲存庫

1. 創建實體類，繼承 `BaseEntity`：

```csharp
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    // 其他屬性...
}
```

2. 將實體添加到 `ApplicationDbContext`：

```csharp
public DbSet<Product> Products { get; set; }
```

3. 使用通用儲存庫操作數據：

```csharp
private readonly IRepository<Product> _productRepository;

public ProductsController(IRepository<Product> productRepository)
{
    _productRepository = productRepository;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    var products = await _productRepository.GetAllAsync();
    return Success(products);
}
```

## 項目結構

- **Controllers/**: API控制器
- **Data/**: 數據訪問相關代碼
  - **Entities/**: 實體類
  - **Repositories/**: 儲存庫實現
- **Models/**: 數據模型和DTO
- **Services/**: 業務邏輯服務
- **Middlewares/**: 中間件組件
- **Extensions/**: 擴展方法
- **Utils/**: 通用工具類
- **Filters/**: API過濾器

## 自定義配置

### 修改JWT設置

在 `appsettings.json` 中的 `JwtSettings` 節點下修改：

```json
"JwtSettings": {
  "Secret": "你的安全密鑰至少32字節長",
  "Issuer": "你的發行方",
  "Audience": "你的接收方",
  "ExpiryMinutes": 60
}
```

### 修改日誌配置

在 `appsettings.json` 中的 `Serilog` 節點下修改：

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "WriteTo": [
    { "Name": "Console" },
    {
      "Name": "File",
      "Args": {
        "path": "Logs/log-.txt",
        "rollingInterval": "Day"
      }
    }
  ]
}
```

## 部署

### 發布應用程序

```bash
dotnet publish -c Release -o ./publish
```

### Docker支持

項目包含基本的Dockerfile，可以使用以下命令構建Docker映像：

```bash
docker build -t dotnet-api-template .
docker run -p 8080:80 dotnet-api-template
```

## 貢獻

歡迎對此模板進行貢獻。請隨時提交問題或拉取請求。

## 許可證

此項目根據MIT許可證授權 - 有關詳細信息，請參閱LICENSE文件。