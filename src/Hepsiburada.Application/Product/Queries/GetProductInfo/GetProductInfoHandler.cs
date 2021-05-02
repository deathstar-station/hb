using System.Linq;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Product.Queries.GetProductInfo
{
    public class GetProductInfoHandler : IRequestHandler<GetProductInfoQuery, GetProductInfoResponse>
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CampaignEntity> _campaignRepository;
        private readonly IProductPriceService _productPriceService;
        private readonly ITimeService _timeService;

        public GetProductInfoHandler(
            IRepository<ProductEntity> productRepository,
            IRepository<CampaignEntity> campaignRepository,
            ITimeService timeService,
            IProductPriceService productPriceService)
        {
            _productRepository = productRepository;
            _campaignRepository = campaignRepository;
            _timeService = timeService;
            _productPriceService = productPriceService;
        }

        public async Task<GetProductInfoResponse> Handle(GetProductInfoQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindOneAsync(x => x.ProductCode == request.ProductCode);
            if (product == null)
                throw new ProductNotFoundException();

            var productCampaigns = await _campaignRepository.FindAsync(x => x.ProductCode == request.ProductCode);

            var productActiveCampaign = productCampaigns.FirstOrDefault(x => x.IsActive(_timeService.CurrentTime));

            var sellingPrice = _productPriceService.CalculateDiscountedPrice(productActiveCampaign, product.Price, _timeService.CurrentTime);

            return new GetProductInfoResponse(product, sellingPrice);
        }
    }
}
