using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Campaign.Commands.CreateCampaign
{
    public class CreateCampaignCommand : IRequest<CreateCampaignResponse>
    {
        public string CampaignName { get; set; }
        public string ProductCode { get; set; }
        public int Duration { get; set; }
        public int PriceManipulationLimit { get; set; }
        public int TargetSalesCount { get; set; }
    }

    public class CreateCampaignValidator : AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignValidator()
        {
            RuleFor(x => x.CampaignName)
                .NotEmpty().WithMessage("Campaign Name cannot be empty")
                .MaximumLength(3).WithMessage("Campaign Name max length must be 3");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage("Product Code cannot be empty")
                .MaximumLength(3).WithMessage("Product Code max length must be 3");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration cannot be less than or equal to zero");

            RuleFor(x => x.TargetSalesCount)
                .GreaterThan(0).WithMessage("Target sales count cannot be less than or equal to zero");

            RuleFor(x => x.PriceManipulationLimit)
                .GreaterThan(0).WithMessage("Price manipulation limit cannot be less than or equal to zero")
                .LessThan(100).WithMessage("Price manipulation limit cannot be greater than 99");
        }
    }
}
