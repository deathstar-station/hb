using FluentValidation;
using MediatR;

namespace Hepsiburada.Application.Campaign.Queries.GetCampaignInfo
{
    public class GetCampaignInfoQuery : IRequest<GetCampaignInfoResponse>
    {
        public string CampaignName { get; set; }
    }

    public class GetCampaignInfoValidator : AbstractValidator<GetCampaignInfoQuery>
    {
        public GetCampaignInfoValidator()
        {
            RuleFor(x => x.CampaignName)
                .NotEmpty().WithMessage("Campaign Name cannot be empty")
                .MaximumLength(3).WithMessage("Campaign Name max length must be 3");
        }
    }
}
