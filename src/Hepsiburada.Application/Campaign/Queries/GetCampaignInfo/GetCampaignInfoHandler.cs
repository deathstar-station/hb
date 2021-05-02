using Hepsiburada.Application.Campaign.Exceptions;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Shared.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Campaign.Queries.GetCampaignInfo
{
    public class GetCampaignInfoHandler : IRequestHandler<GetCampaignInfoQuery, GetCampaignInfoResponse>
    {
        private readonly IRepository<CampaignEntity> _campaignRepository;
        private readonly ITimeService _timeService;

        public GetCampaignInfoHandler(
            IRepository<CampaignEntity> campaignRepository,
            ITimeService timeService)
        {
            _campaignRepository = campaignRepository;
            _timeService = timeService;
        }

        public async Task<GetCampaignInfoResponse> Handle(GetCampaignInfoQuery request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.FindOneAsync(x => x.Name == request.CampaignName);
            if (campaign == null)
                throw new CampaignNotFoundException();

            var result = new GetCampaignInfoResponse
            {
                Name = campaign.Name,
                TargetSales = campaign.TargetSalesCount,
                TotalSales = campaign.TotalSalesCount,
                Turnover = campaign.Turnover,
                AverageItemPrice = campaign.AverageItemPrice,
                IsActive = campaign.IsActive(_timeService.CurrentTime)
            };

            return result;
        }
    }
}
