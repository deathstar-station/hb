using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;

namespace Hepsiburada.Domain.Order
{
    public interface ICreateOrderService
    {
        public OrderEntity Create(ProductEntity product, int quantity, CampaignEntity campaign, int currentTime);
    }
}