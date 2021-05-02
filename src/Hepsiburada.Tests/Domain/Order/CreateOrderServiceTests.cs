using FluentAssertions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Product.Exceptions;
using Moq;
using Xunit;

namespace Hepsiburada.Tests.Domain.Order
{
    public class CreateOrderServiceTests
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly Mock<IProductPriceService> _productPriceServiceMock;
        private readonly Mock<ProductEntity> _productEntityMock;
        private readonly Mock<CampaignEntity> _campaignEntityMock;
        
        public CreateOrderServiceTests()
        {
            _productPriceServiceMock = new Mock<IProductPriceService>();
            _createOrderService = new CreateOrderService(_productPriceServiceMock.Object);
            _productEntityMock = new Mock<ProductEntity>();
            _campaignEntityMock = new Mock<CampaignEntity>();
        }
        
        [Fact]
        public void Create_Should_Throw_Exception_If_Quantity_BiggerThan_Stock()
        {
            var productStock = 10;
            var quantity = 11;

            _productEntityMock.Setup(x => x.Stock).Returns(productStock);

            Assert.Throws<InsufficientStockException>(() =>
            {
                _createOrderService.Create(_productEntityMock.Object, quantity, null, 1);
            });
        }

        [Fact]
        public void Create_Should_Run_Correctly_If_Has_Campaign()
        {
            var productStock = 10;
            var sellingPrice = 100;
            var quantity = 9;
            var currentTime = 0;
            var campaignId = 1;
            var productCode = "P1";

            _productEntityMock.Setup(x => x.Stock).Returns(productStock);
            _productEntityMock.Setup(x => x.ProductCode).Returns(productCode);
            _productEntityMock.Setup(x => x.Price).Returns(sellingPrice);
            _productEntityMock.Setup(x => x.DecreaseStock(It.IsAny<int>()));

            _campaignEntityMock.Setup(x => x.Id).Returns(campaignId);
            _campaignEntityMock.Setup(x => x.NotifyOrderCreation(It.IsAny<int>(), It.IsAny<decimal>()));
            _productPriceServiceMock.Setup(x => x.CalculateDiscountedPrice(It.IsAny<CampaignEntity>(), It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(sellingPrice);

            var order = _createOrderService.Create(_productEntityMock.Object, quantity, _campaignEntityMock.Object, currentTime);

            _productPriceServiceMock.Verify(x => x.CalculateDiscountedPrice(It.IsAny<CampaignEntity>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once);
            _campaignEntityMock.Verify(x => x.NotifyOrderCreation(It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
            _productEntityMock.Verify(x => x.DecreaseStock(It.IsAny<int>()), Times.Once);

            order.Should().NotBeNull();
            order.Price.Should().Be(sellingPrice);
            order.AppliedCampaignId.Should().Be(campaignId);
            order.ProductCode.Should().Be(productCode);
            order.Quantity.Should().Be(quantity);
        }
        
        [Fact]
        public void Create_Should_Run_Correctly_If_Does_Not_Have_Campaign()
        {
            var productStock = 10;
            var sellingPrice = 100;
            var quantity = 9;
            var currentTime = 0;
            var productCode = "P1";

            _productEntityMock.Setup(x => x.Stock).Returns(productStock);
            _productEntityMock.Setup(x => x.ProductCode).Returns(productCode);
            _productEntityMock.Setup(x => x.Price).Returns(sellingPrice);
            _productEntityMock.Setup(x => x.DecreaseStock(It.IsAny<int>()));

            var order = _createOrderService.Create(_productEntityMock.Object, quantity, default(CampaignEntity), currentTime);

            _productPriceServiceMock.Verify(x => x.CalculateDiscountedPrice(It.IsAny<CampaignEntity>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Never);
            _productEntityMock.Verify(x => x.DecreaseStock(It.IsAny<int>()), Times.Once);

            order.Should().NotBeNull();
            order.Price.Should().Be(sellingPrice);
            order.AppliedCampaignId.Should().BeNull();
            order.ProductCode.Should().Be(productCode);
            order.Quantity.Should().Be(quantity);
        }
    }
}
