# 實體模板使用指南

本目錄提供了一組模板文件，用於在您的 .NET API 項目中快速創建新的實體及其相關組件。這些模板遵循最佳實踐，並與項目的架構保持一致。

## 模板目錄結構

```
Templates/
└── ExampleEntity/
    ├── Data/
    │   ├── ExampleEntity.cs                     # 實體類定義
    │   ├── IExampleEntityRepository.cs          # 擴展儲存庫接口（可選）
    │   └── ExampleEntityRepository.cs           # 擴展儲存庫實現（可選）
    ├── Models/
    │   ├── ExampleEntityDto.cs                  # DTO 類定義
    │   └── ExampleEntityValidator.cs            # FluentValidation 驗證器
    ├── Controllers/
    │   ├── ExampleEntitiesController.cs         # 基本控制器
    │   └── ServiceBasedExampleEntitiesController.cs # 使用服務層的控制器
    ├── Services/
    │   ├── IExampleEntityService.cs             # 服務接口（可選）
    │   └── ExampleEntityService.cs              # 服務實現（可選）
    └── MappingProfile.cs                        # AutoMapper 配置
```

## 使用方法

### 創建新實體的步驟

1. **創建實體類**
   
   複製 `ExampleEntity/Data/ExampleEntity.cs` 到 `Data/Entities/` 目錄下，重命名為您的實體名稱（如 `Product.cs`），並修改其屬性。

2. **創建 DTO 類**
   
   複製 `ExampleEntity/Models/ExampleEntityDto.cs` 到 `Models/` 目錄下，重命名為您的實體名稱（如 `ProductDto.cs`），並根據實體類修改其屬性。

3. **創建驗證器**
   
   複製 `ExampleEntity/Models/ExampleEntityValidator.cs` 到 `Models/Validators/` 目錄下（如果目錄不存在，請先創建），重命名為您的實體名稱（如 `ProductValidator.cs`），並根據業務需求修改驗證規則。

4. **添加到 DbContext**
   
   在 `Data/ApplicationDbContext.cs` 中添加 DbSet：
   
   ```csharp
   public DbSet<Product> Products { get; set; }
   ```

5. **添加 AutoMapper 配置**
   
   將 `ExampleEntity/MappingProfile.cs` 中的映射配置添加到項目的 `MappingProfiles.cs` 文件中，並修改為您的實體名稱。

6. **選擇控制器模式**
   
   有兩種控制器模式可供選擇：
   
   a. 基本控制器：直接使用 IRepository<T> 進行數據訪問
      - 複製 `ExampleEntity/Controllers/ExampleEntitiesController.cs` 到 `Controllers/` 目錄
   
   b. 服務層控制器：通過服務層進行數據訪問（推薦用於複雜業務邏輯）
      - 複製 `ExampleEntity/Services/` 下的文件到 `Services/` 目錄
      - 複製 `ExampleEntity/Controllers/ServiceBasedExampleEntitiesController.cs` 到 `Controllers/` 目錄

7. **註冊服務**（如果使用服務層模式）
   
   在 `Program.cs` 中註冊服務：
   
   ```csharp
   // 註冊擴展儲存庫（如果需要）
   builder.Services.AddScoped<IProductRepository, ProductRepository>();
   
   // 註冊服務
   builder.Services.AddScoped<IProductService, ProductService>();
   ```

## 擴展儲存庫（可選）

如果您需要特殊的查詢功能（如按用戶ID查詢），可以：

1. 複製 `ExampleEntity/Data/IExampleEntityRepository.cs` 和 `ExampleEntityRepository.cs` 到 `Data/Repositories/` 目錄
2. 重命名並根據需求修改
3. 在 `Program.cs` 中註冊：

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

## 測試

添加完所有文件後，啟動應用程序並通過 Swagger UI 測試新的 API 端點。

## 注意事項

1. 替換所有 "ExampleEntity" 為您的實體名稱（如 "Product"）
2. 修改屬性和驗證規則以符合業務需求
3. 根據需要添加額外的功能和端點
4. 確保所有命名空間與您的項目匹配

---

通過使用這些模板，您可以快速創建符合項目架構和最佳實踐的新實體及其相關組件，提高開發效率和代碼質量。 