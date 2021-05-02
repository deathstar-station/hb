using FluentAssertions;
using Hepsiburada.Application.Campaign.Commands.CreateCampaign;
using System.Linq;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateCampaign
{
    public class CreateCampaignValidationTests
    {
        private readonly CreateCampaignValidator _validator;

        public CreateCampaignValidationTests()
        {
            _validator = new CreateCampaignValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateCampaign_Should_Not_Validated_When_CampaignName_Is_NullOrEmpty(string campaignName)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = campaignName,
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.CampaignName)).Should().Be(true);
        }

        [Theory]
        [InlineData("AAAAAAAA")]
        public void CreateCampaign_Should_Not_Validated_When_CampaignName_Length_Is_GreaterThan_3(string campaignName)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = campaignName,
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.CampaignName)).Should().Be(true);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateCampaign_Should_Not_Validated_When_ProductCode_Is_NullOrEmpty(string productCode)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3),
                ProductCode = productCode,
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.ProductCode)).Should().Be(true);
        }

        [Theory]
        [InlineData("dDDDDD")]
        [InlineData("Cdfsdsdf32")]
        public void CreateCampaign_Should_Not_Validated_When_ProductCode_Length_Is_GreaterThan_3(string productCode)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3),
                ProductCode = productCode,
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.ProductCode)).Should().Be(true);
        }


        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public void CreateCampaign_Should_Not_Validated_When_Duration_Is_Lower_Than_1(int duration)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3),
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Duration = duration,
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.Duration)).Should().Be(true);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public void CreateCampaign_Should_Not_Validated_When_TargetSalesCount_Is_Lower_Than_1(int targetSalesCount)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3),
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = targetSalesCount,
                PriceManipulationLimit = FakeObjects.Instance.Random.Int(1, 20)
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.TargetSalesCount)).Should().Be(true);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(1250)]
        public void CreateCampaign_Should_Not_Validated_When_PriceManipulationLimit_Is_Lower_Than_1_And_GreaterThan_99(int priceManipulationLImit)
        {
            var createCampaignCommand = new CreateCampaignCommand
            {
                CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3),
                ProductCode = FakeObjects.Instance.Random.AlphaNumeric(3),
                Duration = FakeObjects.Instance.Random.Int(1, 10),
                TargetSalesCount = FakeObjects.Instance.Random.Int(10, 100),
                PriceManipulationLimit = priceManipulationLImit
            };
            var validationResult = _validator.Validate(createCampaignCommand);

            validationResult.Errors.Any(x => x.PropertyName == nameof(createCampaignCommand.PriceManipulationLimit)).Should().Be(true);
        }

    }
}
