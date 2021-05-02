using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Product.Queries.GetProductInfo
{
    public class GetProductInfoQuery : IRequest<GetProductInfoResponse>
    {
        public string ProductCode { get; set; }
    }

    public class GetProductInfoValidator : AbstractValidator<GetProductInfoQuery>
    {
        public GetProductInfoValidator()
        {
            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product Code cannot be empty")
               .MaximumLength(3).WithMessage("Product Code max length must be 3");
        }
    }
}
