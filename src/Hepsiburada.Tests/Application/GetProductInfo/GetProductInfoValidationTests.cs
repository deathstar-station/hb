using System.Linq;
using FluentAssertions;
using Hepsiburada.Application.Product.Queries.GetProductInfo;
using Xunit;

namespace Hepsiburada.Tests.Application.GetProductInfo
{
    public class GetProductInfoValidationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetProductInfo_Should_Not_Validated_When_Product_Code_Is_NullOrEmpty(string productCode)
        {
            var getProductInfoQuery = new GetProductInfoQuery { ProductCode = productCode };
            var validator = new GetProductInfoValidator();
            var validationResult = validator.Validate(getProductInfoQuery);

            validationResult.Errors.Any(x => x.PropertyName == nameof(getProductInfoQuery.ProductCode)).Should().Be(true);
        }

        [Theory]
        [InlineData("CCCC")]
        [InlineData("DDDDDDD")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXX")]
        public void GetProductInfo_Should_Not_Validated_When_Product_Code_Length_Is_GreaterThan_3(string productCode)
        {
            var getProductInfoQuery = new GetProductInfoQuery { ProductCode = productCode };
            var validator = new GetProductInfoValidator();
            var validationResult = validator.Validate(getProductInfoQuery);

            validationResult.Errors.Any(x => x.PropertyName == nameof(getProductInfoQuery.ProductCode)).Should().Be(true);
        }
    }
}
