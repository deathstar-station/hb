using FluentAssertions;
using System.Linq;
using Hepsiburada.Application.Order.Commands.CreateOrder;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateOrder
{
    public class CreateOrderValidationTests
    {
        private readonly CreateOrderValidator _validator;

        public CreateOrderValidationTests()
        {
            _validator = new CreateOrderValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateOrder_Should_Not_Validated_When_ProductCode_Is_NullOrEmpty(string productCode)
        {
            var createOrderCommand = new CreateOrderCommand
            {
                ProductCode = productCode,
                Quantity = FakeObjects.Instance.Random.Int(1, 10)
            };
            var validationResult = _validator.Validate(createOrderCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createOrderCommand.ProductCode)).Should().Be(true);
        }

        [Theory]
        [InlineData("AAAAAAAAA")]
        [InlineData("C1111234345")]
        public void CreateOrder_Should_Not_Validated_When_ProductCode_Length_Is_GreaterThan_3(string productCode)
        {
            var createOrderCommand = new CreateOrderCommand
            {
                ProductCode = productCode,
                Quantity = FakeObjects.Instance.Random.Int(1, 10)
            };
            var validationResult = _validator.Validate(createOrderCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createOrderCommand.ProductCode)).Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void CreateOrder_Should_Not_Validated_When_Quantity_Lower_Than_1(int quantity)
        {
            var createOrderCommand = new CreateOrderCommand
            {
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Quantity = quantity
            };
            var validationResult = _validator.Validate(createOrderCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createOrderCommand.Quantity)).Should().Be(true);
        }
    }
}
