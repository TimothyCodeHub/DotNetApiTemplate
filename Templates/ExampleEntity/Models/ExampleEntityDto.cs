using System;

namespace DotNetApiTemplate.Models
{
    /// <summary>
    /// 讀取實體時的 DTO
    /// </summary>
    public class ExampleEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    /// <summary>
    /// 創建實體時的 DTO
    /// </summary>
    public class CreateExampleEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? CategoryId { get; set; }
    }
    
    /// <summary>
    /// 更新實體時的 DTO
    /// </summary>
    public class UpdateExampleEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public Guid? CategoryId { get; set; }
    }
} 