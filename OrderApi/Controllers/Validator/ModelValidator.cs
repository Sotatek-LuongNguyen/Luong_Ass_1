using FluentValidation;
using OrderApi.Model;
namespace OrderApi.Controllers.Validator
{
    public class ModelValidator : AbstractValidator<Order>
    {
        public ModelValidator()
        {
            RuleFor(x => x.EmployeeName)
                .NotEmpty().WithMessage("Tên nhân viên không được để trống.")
                .MaximumLength(50).WithMessage("Tên nhân viên không được vượt quá 50 ký tự.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Tên khách hàng không được để trống.")
                .MaximumLength(50).WithMessage("Tên khách hàng không được vượt quá 50 ký tự.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống.")
                .MaximumLength(50).WithMessage("Trạng thái không được vượt quá 50 ký tự.");
        }
    }
}
