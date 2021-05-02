using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hepsiburada.Application.Order.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CampaignEntity> _campaignRepository;
        private readonly ICreateOrderService _createOrderService;
        private readonly ITimeService _timeService;

        public CreateOrderHandler(
            IRepository<OrderEntity> orderRepository,
            IRepository<ProductEntity> productRepository,
            IRepository<CampaignEntity> campaignRepository,
            ICreateOrderService createOrderService,
            ITimeService timeService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _campaignRepository = campaignRepository;
            _createOrderService = createOrderService;
            _timeService = timeService;

        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindOneAsync(x => x.ProductCode == request.ProductCode);
            if (product == null) throw new ProductNotFoundException();

            var campaignsForProduct = await _campaignRepository.FindAsync(x => x.ProductCode == request.ProductCode);
            var campaignForProduct = campaignsForProduct.FirstOrDefault(x => x.IsActive(_timeService.CurrentTime));

            var order = _createOrderService.Create(product, request.Quantity, campaignForProduct, _timeService.CurrentTime);

            await _productRepository.UpdateAsync(product);

            if (campaignForProduct != null)
                await _campaignRepository.UpdateAsync(campaignForProduct);

            var orderEntity = await _orderRepository.AddAsync(order);

            return new CreateOrderResponse(orderEntity);
        }


    }
}
