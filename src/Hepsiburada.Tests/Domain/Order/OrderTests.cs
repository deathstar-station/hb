using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Shared.Exceptions;
using Xunit;

namespace Hepsiburada.Tests.Domain.Order
{
    public class OrderTests
    {

        [Theory]
        [InlineData(null, 10, 20)]
        [InlineData(null, 0, 0)]
        public void Order_Should_Throw_Exception_When_ProductCode_Is_Null(string productCode, int quantity, decimal price)
        {
            Assert.Throws<ArgumentRequiredException>(() =>
            {
                new OrderEntity(productCode, quantity, price);
            });
        }

        [Theory]
        [InlineData("C11", 0, 20)]
        [InlineData("C23", -5, 10)]
        public void Order_Should_Throw_Exception_When_Quantity_Is_LessThan_Or_Equal_To_Zero(string productCode, int quantity, decimal price)
        {
            Assert.Throws<ValueMustBeBiggerThanZeroException>(() =>
            {
                new OrderEntity(productCode, quantity, price);
            });
        }

        [Theory]
        [InlineData("C11", 1, 0)]
        [InlineData("C23", 10, -5)]
        [InlineData("C23", 20, -1.5)]
        public void Order_Should_Throw_Exception_When_Price_Is_LessThan_Or_Equal_To_Zero(string productCode, int quantity, decimal price)
        {
            Assert.Throws<ValueMustBeBiggerThanZeroException>(() =>
            {
                new OrderEntity(productCode, quantity, price);
            });
        }

        [Theory]
        [InlineData("P1", 10, 90)]
        [InlineData("P1", 5, 30)]
        [InlineData("OP1", 6, 40)]
        [InlineData("DS1", 8, 0.5)]
        public void Order_Should_Created_Successfully(string productCode, int quantity, decimal price)
        {
            var order = new OrderEntity(productCode, quantity, price);
            Assert.NotNull(order);
        }
        
        [Theory]
        [InlineData("P1", 10, 90, 3)]
        [InlineData("P1", 5, 30, 5)]
        [InlineData("OP1", 6, 40, 7)]
        [InlineData("DS1", 8, 0.5, 9)]
        public void When_Order_Created_With_Campaign_Parameters_Should_Be_Set_Correctly(string productCode, int quantity, decimal price, int campaignId)
        {
            var order = new OrderEntity(productCode, quantity, price, campaignId);

            Assert.Equal(productCode, order.ProductCode);
            Assert.Equal(quantity, order.Quantity);
            Assert.Equal(price, order.Price);
            Assert.Equal(campaignId, order.AppliedCampaignId);
        }


        [Theory]
        [InlineData("P1", 7, 90)]
        [InlineData("P1", 32, 30)]
        [InlineData("OP1", 40, 45)]
        [InlineData("DS1", 60, 21)]
        public void When_Order_Created_Without_Campaign_Parameters_Should_Be_Set_Correctly(string productCode, int quantity, decimal price)
        {
            var order = new OrderEntity(productCode, quantity, price);

            Assert.Equal(productCode, order.ProductCode);
            Assert.Equal(quantity, order.Quantity);
            Assert.Equal(price, order.Price);
        }

    }
}
