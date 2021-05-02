using FluentAssertions;
using System.Linq;
using Hepsiburada.Application.Campaign.Queries.GetCampaignInfo;
using Xunit;

namespace Hepsiburada.Tests.Application.GetCampaignInfo
{
    public class GetCampaignInfoValidationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetCampaignInfo_Should_Not_Validated_When_CampaignName_Is_NullOrEmpty(string campaignName)
        {
            var getCampaignInfo = new GetCampaignInfoQuery { CampaignName = campaignName };
            var validator = new GetCampaignInfoValidator();
            var validationResult = validator.Validate(getCampaignInfo);

            validationResult.Errors.Any(x => x.PropertyName == nameof(getCampaignInfo.CampaignName)).Should().Be(true);
        }

        [Theory]
        [InlineData("CCCC")]
        [InlineData("DDDDDDD")]
        [InlineData("XXXXXXXXXXXXXXXXXXXXXXX")]
        public void GetCampaignInfo_Should_Not_Validated_When_Product_Code_Length_Is_GreaterThan_3(string campaignName)
        {
            var getCampaignInfo = new GetCampaignInfoQuery() { CampaignName = campaignName };
            var validator = new GetCampaignInfoValidator();
            var validationResult = validator.Validate(getCampaignInfo);

            validationResult.Errors.Any(x => x.PropertyName == nameof(getCampaignInfo.CampaignName)).Should().Be(true);
        }
    }
}
