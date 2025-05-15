using System;
using DotNetApiTemplate.Data.Entities;

namespace DotNetApiTemplate.Data.Entities
{
    /// <summary>
    /// 示例實體類 - 將此文件複製到 Data/Entities 目錄下並重命名
    /// </summary>
    public class ExampleEntity : BaseEntity
    {
        // 基本屬性
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public bool IsActive { get; set; }
        
        // 外鍵關係示例
        public Guid? CategoryId { get; set; }
        
        // 用戶關係 - 創建者
        public Guid CreatedById { get; set; }
        
        // 導航屬性示例（如果使用 EF Core）
        // public Category Category { get; set; }
        // public User CreatedBy { get; set; }
    }
} 