using FluentAssertions;
using Hepsiburada.Application.Campaign.Exceptions;
using Hepsiburada.Application.Campaign.Queries.GetCampaignInfo;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Shared.Abstractions;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using Xunit;

namespace Hepsiburada.Tests.Application.GetCampaignInfo
{
    public class GetCampaignInfoQueryTests
    {
        private readonly Mock<IRepository<CampaignEntity>> _campaignRepositoryMock;
        private readonly GetCampaignInfoHandler _handler;

        public GetCampaignInfoQueryTests()
        {
            _campaignRepositoryMock = new Mock<IRepository<CampaignEntity>>();
            var timeServiceMock = new Mock<ITimeService>();

            _handler = new (_campaignRepositoryMock.Object, timeServiceMock.Object);
        }

        [Fact]
        public void GetCampaignInfo_Should_Throw_Exception_When_Campaign_DoesNot_Exists()
        {
            _campaignRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(default(CampaignEntity));

            Assert.ThrowsAsync<CampaignNotFoundException>(async () =>
            {
                var query = new GetCampaignInfoQuery
                {
                    CampaignName = FakeObjects.Instance.Random.AlphaNumeric(3)
                };

                await _handler.Handle(query, CancellationToken.None);
            }).Wait();
        }

        [Fact]
        public void GetCampaignInfo_Should_Run_Correctly()
        {
            var campaign = FakeObjects.GenerateCampaign();

            _campaignRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(campaign);

            var query = new GetCampaignInfoQuery
            {
                CampaignName = campaign.Name
            };

            var result = _handler.Handle(query, CancellationToken.None).Result;

            _campaignRepositoryMock.Verify(x => x.FindOneAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once);

            result.Should().NotBeNull();
            result.Name.Should().Be(campaign.Name);
            result.TargetSales.Should().Be(campaign.TargetSalesCount);
            result.TotalSales.Should().Be(campaign.TotalSalesCount);
            result.AverageItemPrice.Should().Be(campaign.AverageItemPrice);
            result.IsActive.Should().Be(true);
        }
    }
}
