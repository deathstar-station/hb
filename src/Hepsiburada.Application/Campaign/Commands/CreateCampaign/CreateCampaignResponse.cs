using Hepsiburada.Domain.Campaign;

namespace Hepsiburada.Application.Campaign.Commands.CreateCampaign
{
    public class CreateCampaignResponse 
    {
        public CreateCampaignResponse(CampaignEntity campaign)
        {
            Campaign = campaign;
        }

        public CampaignEntity Campaign { get; }
    }
}
