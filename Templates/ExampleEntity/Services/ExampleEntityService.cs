using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetApiTemplate.Data.Entities;
using DotNetApiTemplate.Data.Repositories;
using DotNetApiTemplate.Models;
using Microsoft.Extensions.Logging;

namespace DotNetApiTemplate.Services
{
    /// <summary>
    /// 示例實體服務實現
    /// </summary>
    public class ExampleEntityService : IExampleEntityService
    {
        private readonly IExampleEntityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExampleEntityService> _logger;
        
        public ExampleEntityService(
            IExampleEntityRepository repository,
            IMapper mapper,
            ILogger<ExampleEntityService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary>
        /// 獲取所有實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntityDto>> GetAllAsync()
        {
            _logger.LogInformation("獲取所有示例實體");
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExampleEntityDto>>(entities);
        }
        
        /// <summary>
        /// 根據ID獲取實體
        /// </summary>
        public async Task<ExampleEntityDto> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("獲取ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("未找到ID為 {Id} 的示例實體", id);
                return null;
            }
            
            return _mapper.Map<ExampleEntityDto>(entity);
        }
        
        /// <summary>
        /// 創建新實體
        /// </summary>
        public async Task<ExampleEntityDto> CreateAsync(CreateExampleEntityDto createDto, Guid currentUserId)
        {
            _logger.LogInformation("創建新的示例實體");
            var entity = _mapper.Map<ExampleEntity>(createDto);
            
            // 設置創建者ID
            entity.CreatedById = currentUserId;
            entity.CreatedAt = DateTime.UtcNow;
            
            await _repository.AddAsync(entity);
            
            _logger.LogInformation("已創建ID為 {Id} 的示例實體", entity.Id);
            return _mapper.Map<ExampleEntityDto>(entity);
        }
        
        /// <summary>
        /// 更新現有實體
        /// </summary>
        public async Task<ExampleEntityDto> UpdateAsync(Guid id, UpdateExampleEntityDto updateDto, Guid currentUserId)
        {
            _logger.LogInformation("更新ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            
            if (entity == null)
            {
                _logger.LogWarning("未找到ID為 {Id} 的示例實體", id);
                return null;
            }
            
            // 檢查權限
            if (entity.CreatedById != currentUserId)
            {
                _logger.LogWarning("用戶 {UserId} 嘗試更新非自己創建的實體 {Id}", currentUserId, id);
                throw new UnauthorizedAccessException("您無權修改此實體");
            }
            
            _mapper.Map(updateDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            
            await _repository.UpdateAsync(entity);
            
            _logger.LogInformation("已更新ID為 {Id} 的示例實體", entity.Id);
            return _mapper.Map<ExampleEntityDto>(entity);
        }
        
        /// <summary>
        /// 刪除實體
        /// </summary>
        public async Task DeleteAsync(Guid id, Guid currentUserId)
        {
            _logger.LogInformation("刪除ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            
            if (entity == null)
            {
                _logger.LogWarning("未找到ID為 {Id} 的示例實體", id);
                return;
            }
            
            // 檢查權限
            if (entity.CreatedById != currentUserId)
            {
                _logger.LogWarning("用戶 {UserId} 嘗試刪除非自己創建的實體 {Id}", currentUserId, id);
                throw new UnauthorizedAccessException("您無權刪除此實體");
            }
            
            await _repository.DeleteAsync(id);
            _logger.LogInformation("已刪除ID為 {Id} 的示例實體", id);
        }
        
        /// <summary>
        /// 根據用戶ID獲取實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntityDto>> GetByUserIdAsync(Guid userId)
        {
            _logger.LogInformation("獲取用戶 {UserId} 創建的所有示例實體", userId);
            var entities = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ExampleEntityDto>>(entities);
        }
        
        /// <summary>
        /// 根據名稱搜索實體
        /// </summary>
        public async Task<IEnumerable<ExampleEntityDto>> SearchByNameAsync(string searchTerm)
        {
            _logger.LogInformation("搜索名稱包含 '{SearchTerm}' 的示例實體", searchTerm);
            var entities = await _repository.SearchByNameAsync(searchTerm);
            return _mapper.Map<IEnumerable<ExampleEntityDto>>(entities);
        }
    }
} 