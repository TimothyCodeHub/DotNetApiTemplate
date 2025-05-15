using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetApiTemplate.Data.Entities;
using DotNetApiTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetApiTemplate.Controllers
{
    /// <summary>
    /// 示例實體控制器 - 將此文件複製到 Controllers 目錄下並重命名
    /// </summary>
    public class ExampleEntitiesController : BaseApiController
    {
        private readonly IRepository<ExampleEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExampleEntitiesController> _logger;
        
        public ExampleEntitiesController(
            IRepository<ExampleEntity> repository,
            IMapper mapper,
            ILogger<ExampleEntitiesController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        
        /// <summary>
        /// 獲取所有實體
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("獲取所有示例實體");
            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ExampleEntityDto>>(entities);
            return Success(dtos);
        }
        
        /// <summary>
        /// 獲取指定ID的實體
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("獲取ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound("實體不存在");
                
            var dto = _mapper.Map<ExampleEntityDto>(entity);
            return Success(dto);
        }
        
        /// <summary>
        /// 創建新實體
        /// </summary>
        [HttpPost]
        [Authorize] // 需要認證
        public async Task<IActionResult> Create(CreateExampleEntityDto createDto)
        {
            _logger.LogInformation("創建新的示例實體");
            var entity = _mapper.Map<ExampleEntity>(createDto);
            
            // 設置創建者ID（從當前用戶獲取）
            if (User.Identity.IsAuthenticated)
            {
                // 假設User.FindFirst("sub")獲取的是用戶ID
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    entity.CreatedById = userId;
                }
            }
            
            entity.CreatedAt = DateTime.UtcNow;
            
            await _repository.AddAsync(entity);
            
            var resultDto = _mapper.Map<ExampleEntityDto>(entity);
            return Created(resultDto);
        }
        
        /// <summary>
        /// 更新現有實體
        /// </summary>
        [HttpPut("{id}")]
        [Authorize] // 需要認證
        public async Task<IActionResult> Update(Guid id, UpdateExampleEntityDto updateDto)
        {
            _logger.LogInformation("更新ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound("實體不存在");
                
            // 檢查是否是創建者（可選，視業務需求而定）
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    if (entity.CreatedById != userId)
                    {
                        return Unauthorized("您無權修改此實體");
                    }
                }
            }
            
            // 使用AutoMapper更新實體
            _mapper.Map(updateDto, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            
            await _repository.UpdateAsync(entity);
            
            var resultDto = _mapper.Map<ExampleEntityDto>(entity);
            return Success(resultDto);
        }
        
        /// <summary>
        /// 刪除指定ID的實體
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize] // 需要認證
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("刪除ID為 {Id} 的示例實體", id);
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return NotFound("實體不存在");
                
            // 檢查是否是創建者（可選，視業務需求而定）
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    if (entity.CreatedById != userId)
                    {
                        return Unauthorized("您無權刪除此實體");
                    }
                }
            }
            
            await _repository.DeleteAsync(entity);
            
            return NoContent();
        }
        
        /// <summary>
        /// 獲取用戶創建的所有實體
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            _logger.LogInformation("獲取用戶 {UserId} 創建的所有示例實體", userId);
            
            // 由於泛型儲存庫沒有按用戶ID過濾的方法，需要先獲取所有實體再過濾
            // 在實際項目中，建議擴展 IRepository 或創建專用的儲存庫
            var allEntities = await _repository.GetAllAsync();
            var userEntities = allEntities.Where(e => e.CreatedById == userId).ToList();
            
            var dtos = _mapper.Map<IEnumerable<ExampleEntityDto>>(userEntities);
            return Success(dtos);
        }
    }
} 