using FluentValidation;
using OrderApi.Dto;

namespace OrderApi.Controllers.Validator
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(o => o.Quantity)
            .GreaterThan(0)
            .WithMessage("Số lượng phải lớn hơn 0");

            RuleFor(o => o.Price)
                .GreaterThan(0)
                .WithMessage("Giá phải lớn hơn 0");

            RuleFor(o => o.IdUser)
                .GreaterThanOrEqualTo(0)
                .WithMessage("IdUser phải lớn hơn hoặc bằng 0");

            RuleFor(o => o.IdEmployee)
                .GreaterThanOrEqualTo(0)
                .WithMessage("IdEmployee phải lớn hơn hoặc bằng 0");

            RuleFor(o => o.IdProduct)
                .GreaterThanOrEqualTo(0)
                .WithMessage("IdProduct phải lớn hơn hoặc bằng 0");

            RuleFor(o => o.Status)
                .MaximumLength(50)
                .WithMessage("Trạng thái không được dài quá 50 ký tự");
        }
    }
}
