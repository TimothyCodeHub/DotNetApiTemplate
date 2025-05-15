using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetApiTemplate.Data.Entities;

namespace DotNetApiTemplate.Data.Repositories
{
    /// <summary>
    /// 擴展的示例實體儲存庫接口 - 如果需要特殊查詢，可以創建此接口
    /// </summary>
    public interface IExampleEntityRepository : IRepository<ExampleEntity>
    {
        /// <summary>
        /// 根據用戶ID查詢實體
        /// </summary>
        Task<IEnumerable<ExampleEntity>> GetByUserIdAsync(Guid userId);
        
        /// <summary>
        /// 根據名稱搜索實體
        /// </summary>
        Task<IEnumerable<ExampleEntity>> SearchByNameAsync(string searchTerm);
        
        /// <summary>
        /// 根據類別ID查詢實體
        /// </summary>
        Task<IEnumerable<ExampleEntity>> GetByCategoryIdAsync(Guid categoryId);
        
        /// <summary>
        /// 獲取活動的實體
        /// </summary>
        Task<IEnumerable<ExampleEntity>> GetActiveEntitiesAsync();
    }
} 