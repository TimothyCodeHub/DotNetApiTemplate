using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetApiTemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetApiTemplate.Data.Repositories
{
    /// <summary>
    /// 擴展的示例實體儲存庫實現
    /// </summary>
    public class ExampleEntityRepository : Repository<ExampleEntity>, IExampleEntityRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ExampleEntityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        /// <summary>
        /// 根據用戶ID查詢實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntity>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<ExampleEntity>()
                .Where(e => e.CreatedById == userId)
                .ToListAsync();
        }
        
        /// <summary>
        /// 根據名稱搜索實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntity>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();
                
            return await _context.Set<ExampleEntity>()
                .Where(e => e.Name.Contains(searchTerm))
                .ToListAsync();
        }
        
        /// <summary>
        /// 根據類別ID查詢實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntity>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Set<ExampleEntity>()
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }
        
        /// <summary>
        /// 獲取活動的實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntity>> GetActiveEntitiesAsync()
        {
            return await _context.Set<ExampleEntity>()
                .Where(e => e.IsActive)
                .ToListAsync();
        }
    }
} 