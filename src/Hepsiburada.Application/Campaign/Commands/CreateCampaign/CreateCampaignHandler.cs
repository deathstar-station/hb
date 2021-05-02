using Hepsiburada.Application.Campaign.Exceptions;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Campaign.Commands.CreateCampaign
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, CreateCampaignResponse>
    {
        private readonly IRepository<CampaignEntity> _campaignRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly ITimeService _timeService;

        public CreateCampaignHandler(IRepository<CampaignEntity> campaignRepository, IRepository<ProductEntity> productRepository, ITimeService timeService)
        {
            _campaignRepository = campaignRepository;
            _productRepository = productRepository;
            _timeService = timeService;
        }

        public async Task<CreateCampaignResponse> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            var productIsExists = await _productRepository.AnyAsync(x => x.ProductCode == request.ProductCode);
            if (!productIsExists)
                throw new ProductNotFoundException();

            var productCampaigns= await _campaignRepository.FindAsync(x => x.ProductCode == request.ProductCode);
            var isAnyActiveCampaign = productCampaigns.Any(x => x.IsActive(_timeService.CurrentTime));
            if (isAnyActiveCampaign)
                throw new AlreadyActiveCampaignForProductException(request.ProductCode);

            var isCampaignExists = await _campaignRepository.AnyAsync((x => x.Name == request.CampaignName));
            if (isCampaignExists)
                throw new CampaignAlreadyExistsException(request.CampaignName);

            var campaign = new CampaignEntity(request.CampaignName, request.ProductCode, request.Duration, request.PriceManipulationLimit, request.TargetSalesCount, _timeService.CurrentTime);

            var result = await _campaignRepository.AddAsync(campaign);

            return new CreateCampaignResponse(result);
        }
    }
}
