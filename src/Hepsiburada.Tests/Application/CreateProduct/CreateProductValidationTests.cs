using FluentAssertions;
using Hepsiburada.Application.Product.Commands.CreateProduct;
using System.Linq;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateProduct
{
    public class CreateProductValidationTests
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidationTests()
        {
            _validator = new CreateProductValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateProduct_Should_Not_Validated_When_Product_Code_Is_Null_Or_Empty(string productCode)
        {
            var createProductCommand = new CreateProductCommand
            {
                ProductCode = productCode,
                Price = FakeObjects.Instance.Random.Decimal(10, 100),
                Stock = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createProductCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createProductCommand.ProductCode)).Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10.5D)]
        public void CreateProduct_Should_Not_Validated_When_Price_Is_Zero_Or_Negative(decimal price)
        {
            var createProductCommand = new CreateProductCommand
            {
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Price = price,
                Stock = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createProductCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createProductCommand.Price)).Should().Be(true);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(int.MinValue)]
        public void CreateProduct_Should_Not_Validated_When_Stock_Is_Negative(int stock)
        {
            var createProductCommand = new CreateProductCommand
            {
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Price = FakeObjects.Instance.Random.Decimal(10, 100),
                Stock = stock
            };
            var validationResult = _validator.Validate(createProductCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createProductCommand.Stock)).Should().Be(true);
        }

    }
}
