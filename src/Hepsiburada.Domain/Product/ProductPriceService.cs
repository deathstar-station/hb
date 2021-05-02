using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Shared.Exceptions;

namespace Hepsiburada.Domain.Product
{
    public class ProductPriceService : IProductPriceService
    {
        public decimal CalculateDiscountedPrice(CampaignEntity campaign, decimal listingPrice, int currentTime)
        {
            if (listingPrice <= 0)
                throw new ValueMustBeBiggerThanZeroException(nameof(listingPrice), listingPrice);

            if (campaign == null || !campaign.IsActive(currentTime))
                return listingPrice;

            var manipulationPercentageByHour = campaign.PriceManipulationLimit / campaign.Duration;
            var discountPercentage = manipulationPercentageByHour * (currentTime - campaign.CreatedTime + 1); //saat 0 oldugunda hesaplama hatasi olmamasi icin +1 ekleniyor
            return listingPrice - listingPrice / 100 * discountPercentage;
        }
    }
}
