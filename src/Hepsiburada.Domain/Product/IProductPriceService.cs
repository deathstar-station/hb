using Hepsiburada.Domain.Campaign;

namespace Hepsiburada.Domain.Product
{
    public interface IProductPriceService
    {
        public decimal CalculateDiscountedPrice(CampaignEntity campaign,decimal listingPrice, int currentTime);
    }
}