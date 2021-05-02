using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Product.Exceptions;
using Hepsiburada.Domain.Shared.Exceptions;
using Xunit;

namespace Hepsiburada.Tests.Domain.Product
{
    public class ProductTests
    {
        [Theory]
        [InlineData(null, 10, 20)]
        public void Product_Should_Throw_Exception_When_ProductCode_Is_Null(string productCode, decimal price, int stock)
        {
            Assert.Throws<ArgumentRequiredException>(() =>
            {
                new ProductEntity(productCode, price, stock);
            });
        }

        [Theory]
        [InlineData("C1", 0, 20)]
        [InlineData("C1", -5, 20)]
        public void Product_Should_Throw_Exception_When_Price_Is_LessThan_Or_Equal_To_Zero(string productCode, decimal price, int stock)
        {
            Assert.Throws<ValueMustBeBiggerThanZeroException>(() =>
            {
                new ProductEntity(productCode, price, stock);
            });
        }

        [Theory]
        [InlineData("C1", 10, -5)]
        public void Product_Should_Throw_Exception_When_Stock_Is_LessThan_Zero(string productCode, decimal price, int stock)
        {
            Assert.Throws<ValueCannotBeLowerThanZeroException>(() =>
            {
                new ProductEntity(productCode, price, stock);
            });
        }

        [Theory]
        [InlineData("C1", 10, 0)]
        [InlineData("C1", 5, 1000)]
        [InlineData("C1", 0.5, 1000)]
        [InlineData("C1", 0.1, 1)]
        public void Product_Should_Created_Successfully(string productCode, decimal price, int stock)
        {
            var product = new ProductEntity(productCode, price, stock);
            Assert.NotNull(product);
        }

        [Theory]
        [InlineData("C6", 10, 10, 20)]
        [InlineData("C2", 10, 39, 40)]
        public void Product_Should_Throw_Exception_When_Quantity_LessThan_Stock(string productCode, decimal price, int stock, int quantity)
        {
            Assert.Throws<InsufficientStockException>(() =>
            {
                var product = new ProductEntity(productCode, price, stock);
                product.DecreaseStock(quantity);
            });
        }

        [Theory]
        [InlineData("C5", 10, 10, 10)]
        [InlineData("C4", 10, 39, 38)]
        public void Product_When_Quantity_Is_Equal_Or_LessThan_Stock_Calculate_Correctly(string productCode, decimal price, int stock, int quantity)
        {
            var product = new ProductEntity(productCode, price, stock);
            product.DecreaseStock(quantity);

            Assert.Equal(stock - quantity, product.Stock);
        }
        
        [Theory]
        [InlineData("C6", 10, 10)]
        [InlineData("C2", 25, 39)]
        public void When_Product_Created_Parameters_Should_Be_Set_Correctly(string productCode, decimal price, int stock)
        {
            var product = new ProductEntity(productCode, price, stock);

            Assert.Equal(productCode, product.ProductCode);
            Assert.Equal(price, product.Price);
            Assert.Equal(stock, product.Stock);
        }
    }
}
