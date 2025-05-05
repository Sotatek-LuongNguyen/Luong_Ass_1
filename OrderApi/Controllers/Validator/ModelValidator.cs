using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderApi.Model;
namespace OrderApi.Controllers.Validator
{
    public class ModelValidator : AbstractValidator<Order>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id không được để trống.")
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(50).WithMessage("Trạng thái không được vượt quá 50 ký tự.");
            
        }
    }
}
