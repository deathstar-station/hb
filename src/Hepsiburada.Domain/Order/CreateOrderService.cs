using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Product.Exceptions;

namespace Hepsiburada.Domain.Order
{
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IProductPriceService _productPriceService;

        public CreateOrderService(IProductPriceService productPriceService)
        {
            _productPriceService = productPriceService;
        }

        public OrderEntity Create(ProductEntity product, int quantity, CampaignEntity campaign, int currentTime)
        {
            if (product.Stock < quantity)
                throw new InsufficientStockException();

            OrderEntity order;
            if (campaign != null)
            {
                var sellingPrice = _productPriceService.CalculateDiscountedPrice(campaign, product.Price, currentTime);

                order = new OrderEntity(product.ProductCode, quantity, sellingPrice, campaign.Id);

                campaign.NotifyOrderCreation(quantity, order.Price);
            }
            else
            {
                order = new OrderEntity(product.ProductCode, quantity, product.Price);
            }

            product.DecreaseStock(quantity);

            return order;
        }
    }
}
