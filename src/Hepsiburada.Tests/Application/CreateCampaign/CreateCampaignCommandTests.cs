using Bogus;
using FluentAssertions;
using Hepsiburada.Application.Campaign.Commands.CreateCampaign;
using Hepsiburada.Application.Campaign.Exceptions;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Moq;
using MoqExpression;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateCampaign
{
    public class CreateCampaignCommandTests
    {
        private readonly Mock<IRepository<CampaignEntity>> _campaignRepositoryMock;
        private readonly Mock<IRepository<ProductEntity>> _productRepositoryMock;
        private readonly CreateCampaignHandler _handler;

        private readonly Faker<CreateCampaignCommand> _createCampaignCommandFaker;

        public CreateCampaignCommandTests()
        {
            _campaignRepositoryMock = new Mock<IRepository<CampaignEntity>>();
            _productRepositoryMock = new Mock<IRepository<ProductEntity>>();
            var timeServiceMock = new Mock<ITimeService>();

            _createCampaignCommandFaker = new Faker<CreateCampaignCommand>()
                .RuleFor(x => x.CampaignName, f => f.Random.AlphaNumeric(3))
                .RuleFor(x => x.Duration, f => f.Random.Int(1, 10))
                .RuleFor(x => x.ProductCode, f => f.Random.AlphaNumeric(3))
                .RuleFor(x => x.PriceManipulationLimit, f => f.Random.Int(1, 20))
                .RuleFor(x => x.TargetSalesCount, f => f.Random.Int(10, 100));

            _handler = new(_campaignRepositoryMock.Object, _productRepositoryMock.Object, timeServiceMock.Object);
        }


        [Fact]
        public void CreateCampaign_Command_ShouldBe_Throw_Exception_When_ProductNotExists()
        {
            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(false);

            Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                await _handler.Handle(_createCampaignCommandFaker.Generate(), CancellationToken.None);
            }).Wait();
        }

        [Fact]
        public void CreateCampaign_Command_ShouldBe_Throw_Exception_When_There_Is_ActiveCampaign_For_SameProduct()
        {
            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(true);

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity> { FakeObjects.GenerateCampaign() });

            Assert.ThrowsAsync<AlreadyActiveCampaignForProductException>(async () =>
           {
               await _handler.Handle(_createCampaignCommandFaker.Generate(), CancellationToken.None);
           }).Wait();
        }

        [Fact]
        public void CreateCampaign_Command_ShouldBe_Throw_Exception_When_There_Is_Campaign_With_SameName()
        {
            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(true);

            var command = _createCampaignCommandFaker.Generate();

            _campaignRepositoryMock.Setup(x => x.AnyAsync(MoqHelper.IsExpression<CampaignEntity>(campaign => campaign.Name == command.CampaignName)))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<CampaignAlreadyExistsException>(async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            }).Wait();
        }

        [Fact]
        public void CreateCampaign_Command_Should_Run_Correctly()
        {
            var command = _createCampaignCommandFaker.Generate();

            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(true);

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity>());

            _campaignRepositoryMock.Setup(x => x.AnyAsync(MoqHelper.IsExpression<CampaignEntity>(campaign => campaign.Name == command.CampaignName)))
                .ReturnsAsync(false);

            _campaignRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CampaignEntity>()))
                .ReturnsAsync(FakeObjects.GenerateCampaign());

            var result = _handler.Handle(command, CancellationToken.None).Result;

            result.Campaign.Should().NotBeNull();

            _productRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CampaignEntity>()), Times.Once);
        }
    }

}
