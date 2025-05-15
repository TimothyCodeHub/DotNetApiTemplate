using FluentValidation;
using DotNetApiTemplate.Models;

namespace DotNetApiTemplate.Models.Validators
{
    /// <summary>
    /// 創建實體時的驗證器
    /// </summary>
    public class CreateExampleEntityDtoValidator : AbstractValidator<CreateExampleEntityDto>
    {
        public CreateExampleEntityDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("名稱不能為空")
                .MaximumLength(100).WithMessage("名稱不能超過100個字符");
                
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("價格不能為空")
                .GreaterThan(0).WithMessage("價格必須大於零");
                
            // 可選的外鍵驗證
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("類別不能為空")
                .When(x => x.CategoryId.HasValue); // 只有當提供了類別ID時才驗證
        }
    }
    
    /// <summary>
    /// 更新實體時的驗證器
    /// </summary>
    public class UpdateExampleEntityDtoValidator : AbstractValidator<UpdateExampleEntityDto>
    {
        public UpdateExampleEntityDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("名稱不能為空")
                .MaximumLength(100).WithMessage("名稱不能超過100個字符")
                .When(x => x.Name != null); // 只有當提供了名稱時才驗證
                
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("價格不能為空")
                .GreaterThan(0).WithMessage("價格必須大於零")
                .When(x => x.Price != 0); // 只有當提供了價格時才驗證
        }
    }
} 