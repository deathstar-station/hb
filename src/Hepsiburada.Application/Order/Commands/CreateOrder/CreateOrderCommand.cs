using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product Code cannot be empty")
                .MaximumLength(3).WithMessage("Product Code max length must be 3");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity cannot be less than or equal to zero ");
        }
    }
}
