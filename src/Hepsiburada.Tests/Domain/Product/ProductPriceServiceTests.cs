using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Exceptions;
using Moq;
using Xunit;

namespace Hepsiburada.Tests.Domain.Product
{
    public class ProductPriceServiceTests
    {
        private readonly IProductPriceService _productPriceService;

        public ProductPriceServiceTests()
        {
            _productPriceService = new ProductPriceService();
        }

        [Theory]
        [InlineData(100)]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(4)]
        public void CalculateDiscountedPrice_Should_Return_listingPrice_If_Campaign_Null_Or_Ended(decimal price)
        {
            var sellingPrice = _productPriceService.CalculateDiscountedPrice(null, price, 0);
            Assert.Equal(price, sellingPrice);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(-0.5)]
        public void CalculateDiscountedPrice_Should_Throw_Exception_If_Price_Lower_Than_Or_EqualTo_Zero(decimal price)
        {
            Assert.Throws<ValueMustBeBiggerThanZeroException>(() =>
            {
                _productPriceService.CalculateDiscountedPrice(null, price, 0);
            });
        }

        [Theory]
        [InlineData(10, 20, 2, 100, 94)]
        [InlineData(10, 20, 1, 100, 96)]
        [InlineData(10, 20, 3, 100, 92)]
        public void CalculateDiscountedPrice_Should_Return_Selling_Price_Correctly(int duration, int priceManipulationLimit, int currentTime, decimal listingPrice, decimal expectedPrice)
        {
            var campaignMock = new Mock<CampaignEntity>();
            campaignMock.Setup(x => x.IsActive(It.IsAny<int>())).Returns(true);

            campaignMock.Setup(x => x.Duration).Returns(duration);
            campaignMock.Setup(x => x.PriceManipulationLimit).Returns(priceManipulationLimit);
            campaignMock.Setup(x => x.CreatedTime).Returns(0);

            var sellingPrice = _productPriceService.CalculateDiscountedPrice(campaignMock.Object, listingPrice, currentTime);
            Assert.Equal(expectedPrice, sellingPrice);
        }

    }
}
