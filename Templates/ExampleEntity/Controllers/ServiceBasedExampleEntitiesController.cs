using System;
using System.Threading.Tasks;
using DotNetApiTemplate.Models;
using DotNetApiTemplate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetApiTemplate.Controllers
{
    /// <summary>
    /// 基於服務層的示例實體控制器 - 當業務邏輯複雜時，建議使用此模式
    /// </summary>
    public class ServiceBasedExampleEntitiesController : BaseApiController
    {
        private readonly IExampleEntityService _exampleEntityService;
        private readonly ILogger<ServiceBasedExampleEntitiesController> _logger;
        
        public ServiceBasedExampleEntitiesController(
            IExampleEntityService exampleEntityService,
            ILogger<ServiceBasedExampleEntitiesController> logger)
        {
            _exampleEntityService = exampleEntityService;
            _logger = logger;
        }
        
        /// <summary>
        /// 獲取所有實體
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _exampleEntityService.GetAllAsync();
            return Success(entities);
        }
        
        /// <summary>
        /// 獲取指定ID的實體
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await _exampleEntityService.GetByIdAsync(id);
            if (entity == null)
                return NotFound("實體不存在");
                
            return Success(entity);
        }
        
        /// <summary>
        /// 創建新實體
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateExampleEntityDto createDto)
        {
            try
            {
                // 獲取當前用戶ID
                var currentUserId = GetCurrentUserId();
                
                var createdEntity = await _exampleEntityService.CreateAsync(createDto, currentUserId);
                return Created(createdEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建實體時發生錯誤");
                return ServerError("創建實體時發生錯誤");
            }
        }
        
        /// <summary>
        /// 更新現有實體
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, UpdateExampleEntityDto updateDto)
        {
            try
            {
                // 獲取當前用戶ID
                var currentUserId = GetCurrentUserId();
                
                var updatedEntity = await _exampleEntityService.UpdateAsync(id, updateDto, currentUserId);
                if (updatedEntity == null)
                    return NotFound("實體不存在");
                    
                return Success(updatedEntity);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("您無權修改此實體");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新實體時發生錯誤");
                return ServerError("更新實體時發生錯誤");
            }
        }
        
        /// <summary>
        /// 刪除指定ID的實體
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // 獲取當前用戶ID
                var currentUserId = GetCurrentUserId();
                
                await _exampleEntityService.DeleteAsync(id, currentUserId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("您無權刪除此實體");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除實體時發生錯誤");
                return ServerError("刪除實體時發生錯誤");
            }
        }
        
        /// <summary>
        /// 獲取用戶創建的所有實體
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var entities = await _exampleEntityService.GetByUserIdAsync(userId);
            return Success(entities);
        }
        
        /// <summary>
        /// 搜索實體
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var entities = await _exampleEntityService.SearchByNameAsync(searchTerm);
            return Success(entities);
        }
        
        /// <summary>
        /// 獲取當前用戶ID的輔助方法
        /// </summary>
        private Guid GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }
            }
            
            throw new UnauthorizedAccessException("無法獲取當前用戶ID");
        }
    }
} 