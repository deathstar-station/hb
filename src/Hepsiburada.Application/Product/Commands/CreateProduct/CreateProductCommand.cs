using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Product.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductResponse>
    {
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product Code cannot be empty")
                .MaximumLength(3).WithMessage("Product Code max length must be 3");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price cannot be less than or equal to zero");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be less than or equal to zero ");
        }
    }
}
