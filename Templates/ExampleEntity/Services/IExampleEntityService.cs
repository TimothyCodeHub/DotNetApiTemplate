using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetApiTemplate.Models;

namespace DotNetApiTemplate.Services
{
    /// <summary>
    /// 示例實體服務接口 - 如果需要業務邏輯層，可以創建此接口
    /// </summary>
    public interface IExampleEntityService
    {
        /// <summary>
        /// 獲取所有實體
        /// </summary>
        Task<IEnumerable<ExampleEntityDto>> GetAllAsync();
        
        /// <summary>
        /// 根據ID獲取實體
        /// </summary>
        Task<ExampleEntityDto> GetByIdAsync(Guid id);
        
        /// <summary>
        /// 創建新實體
        /// </summary>
        Task<ExampleEntityDto> CreateAsync(CreateExampleEntityDto createDto, Guid currentUserId);
        
        /// <summary>
        /// 更新現有實體
        /// </summary>
        Task<ExampleEntityDto> UpdateAsync(Guid id, UpdateExampleEntityDto updateDto, Guid currentUserId);
        
        /// <summary>
        /// 刪除實體
        /// </summary>
        Task DeleteAsync(Guid id, Guid currentUserId);
        
        /// <summary>
        /// 根據用戶ID獲取實體
        /// </summary>
        Task<IEnumerable<ExampleEntityDto>> GetByUserIdAsync(Guid userId);
        
        /// <summary>
        /// 根據名稱搜索實體
        /// </summary>
        Task<IEnumerable<ExampleEntityDto>> SearchByNameAsync(string searchTerm);
    }
} 