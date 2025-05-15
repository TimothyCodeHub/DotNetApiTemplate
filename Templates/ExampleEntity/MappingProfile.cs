using AutoMapper;
using DotNetApiTemplate.Data.Entities;
using DotNetApiTemplate.Models;

namespace DotNetApiTemplate
{
    /// <summary>
    /// AutoMapper 配置示例 - 將此代碼片段添加到項目的 MappingProfiles 類中
    /// </summary>
    public class ExampleEntityMappingProfile : Profile
    {
        public ExampleEntityMappingProfile()
        {
            // 實體到 DTO 的映射
            CreateMap<ExampleEntity, ExampleEntityDto>();
            
            // DTO 到實體的映射
            CreateMap<CreateExampleEntityDto, ExampleEntity>();
            CreateMap<UpdateExampleEntityDto, ExampleEntity>()
                .ForAllMembers(opts => opts.Condition(
                    (src, dest, srcMember) => srcMember != null));
        }
    }
} 