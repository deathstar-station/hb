using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Campaign.Exceptions;
using Hepsiburada.Domain.Shared.Exceptions;
using Xunit;

namespace Hepsiburada.Tests.Domain.Campaign
{
    public class CampaignTests
    {

        [Theory]
        [InlineData(null, "c1", 10, 20, 20, 0)]
        [InlineData(null, "c1", 0, 0, 20, 0)]
        public void Campaign_Should_Throw_Exception_When_Name_Is_Null(string campaignName, string productCode, int duration, int priceManipulationLimit, int targetSalesCount, int time)
        {
            Assert.Throws<ArgumentRequiredException>(() =>
            {
                new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, time);
            });
        }

        [Theory]
        [InlineData("C1", null, 10, 20, 20, 0)]
        [InlineData("C2", null, 0, 0, 20, 0)]
        public void Campaign_Should_Throw_Exception_When_ProductCode_Is_Null(string campaignName, string productCode, int duration, int priceManipulationLimit, int targetSalesCount, int time)
        {
            Assert.Throws<ArgumentRequiredException>(() =>
            {
                new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, time);
            });
        }

        [Theory]
        [InlineData("C1", "P13", 0, 20, 20, 0)]
        [InlineData("C2", "P24", -5, 0, 20, 0)]
        public void Campaign_Should_Throw_Exception_When_Duration_Is_Lower_Than_Or_EqualTo_Zero(
            string campaignName,
            string productCode,
            int duration,
            int priceManipulationLimit,
            int targetSalesCount,
            int time)
        {
            Assert.Throws<InvalidDurationException>(() =>
            {
                new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, time);
            });
        }

        [Theory]
        [InlineData("C1", "P13", 10, -5, 20, 0)]
        [InlineData("C2", "P24", 5, 0, 20, 0)]
        public void Campaign_Should_Throw_Exception_When_PriceManipulationLimit_Is_Lower_Than_Or_EqualTo_Zero(
            string campaignName,
            string productCode,
            int duration,
            int priceManipulationLimit,
            int targetSalesCount,
            int time)
        {
            Assert.Throws<InvalidPriceManipulationLimitException>(() =>
            {
                new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, time);
            });
        }

        [Theory]
        [InlineData("C1", "P13", 10, 10, 0, 0)]
        [InlineData("C2", "P24", 5, 5, -2, 0)]
        public void Campaign_Should_Throw_Exception_When_TargetSalesCount_Is_Lower_Than_Or_EqualTo_Zero(
            string campaignName,
            string productCode,
            int duration,
            int priceManipulationLimit,
            int targetSalesCount,
            int time)
        {
            Assert.Throws<ValueMustBeBiggerThanZeroException>(() =>
            {
                new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, time);
            });
        }

        [Fact]
        public void Campaign_Should_CalculateCorrectly_AverageItemPrice_And_Turnover()
        {
            var campaign = new CampaignEntity("C1", "P1", 10, 10, 10, 0);
            campaign.NotifyOrderCreation(10, 25.5M);
            campaign.NotifyOrderCreation(5, 50);
            campaign.NotifyOrderCreation(40, 60);

            Assert.Equal(52, campaign.AverageItemPrice);
            Assert.Equal(2905, campaign.Turnover);
        }


        [Fact]
        public void When_Campaign_Expired_IsActive_Must_Be_Return_False()
        {
            var campaign = new CampaignEntity("C1", "P1", 10, 10, 10, 0);

            var isActive = campaign.IsActive(10);

            Assert.False(isActive);
        }

        [Fact]
        public void When_Campaign_Create_IsActive_Return_True()
        {
            var campaign = new CampaignEntity("C1", "P1", 10, 10, 10, 0);

            var isActive = campaign.IsActive(4);

            Assert.True(isActive);
        }


        [Theory]
        [InlineData("C1", "P1", 10, 20, 20, 0)]
        [InlineData("C2", "P2", 15, 22, 20, 1)]
        public void When_Campaign_Created_Parameters_Should_Be_Set_Correctly(
            string campaignName,
            string productCode,
            int duration,
            int priceManipulationLimit,
            int targetSalesCount,
            int createdTime)
        {
            var campaign = new CampaignEntity(campaignName, productCode, duration, priceManipulationLimit, targetSalesCount, createdTime);
            Assert.Equal(campaignName, campaign.Name);
            Assert.Equal(productCode, campaign.ProductCode);
            Assert.Equal(duration, campaign.Duration);
            Assert.Equal(priceManipulationLimit, campaign.PriceManipulationLimit);
            Assert.Equal(targetSalesCount, campaign.TargetSalesCount);
            Assert.Equal(createdTime, campaign.CreatedTime);

        }

    }
}
