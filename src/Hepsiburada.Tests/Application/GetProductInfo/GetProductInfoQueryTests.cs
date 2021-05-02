using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Bogus;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Application.Product.Queries.GetProductInfo;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Xunit;

namespace Hepsiburada.Tests.Application.GetProductInfo
{
    public class GetProductInfoQueryTests
    {

        private readonly Mock<IRepository<ProductEntity>> _productRepositoryMock;
        private readonly Mock<IRepository<CampaignEntity>> _campaignRepositoryMock;
        private readonly Mock<IProductPriceService> _productPriceServiceMock;
        private readonly GetProductInfoHandler _handler;

        private readonly Faker<GetProductInfoQuery> _getProductInfoFaker;

        public GetProductInfoQueryTests()
        {
            _productRepositoryMock = new Mock<IRepository<ProductEntity>>();
            _campaignRepositoryMock = new Mock<IRepository<CampaignEntity>>();
            var timeServiceMock = new Mock<ITimeService>();
            _productPriceServiceMock = new Mock<IProductPriceService>();

            _handler = new(_productRepositoryMock.Object, _campaignRepositoryMock.Object, timeServiceMock.Object,
                _productPriceServiceMock.Object);

            _getProductInfoFaker = new Faker<GetProductInfoQuery>()
                .RuleFor(x => x.ProductCode, f => f.Random.AlphaNumeric(3));
        }


        [Fact]
        public void GetProductInfo_Should_Throw_Exception_When_Product_Not_Exists()
        {
            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(default(ProductEntity));

            Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                await _handler.Handle(_getProductInfoFaker.Generate(), CancellationToken.None);
            });
        }

        [Fact]
        public void GetProductInfo_Should_Run_Correctly_If_Dont_Have_Campaign()
        {
            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).
                ReturnsAsync(FakeObjects.GenerateProduct());

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity>());

            var result = _handler.Handle(_getProductInfoFaker.Generate(), CancellationToken.None).Result;

            _productRepositoryMock.Verify(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once());
            _productPriceServiceMock.Verify(x=>x.CalculateDiscountedPrice(It.IsAny<CampaignEntity>(),It.IsAny<decimal>(),It.IsAny<int>()),Times.Once);

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetProductInfo_Should_Run_Correctly_If_Have_Campaign()
        {

            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>())).
                ReturnsAsync(FakeObjects.GenerateProduct);

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity>() { FakeObjects.GenerateCampaign() });

            var result = _handler.Handle(_getProductInfoFaker.Generate(), CancellationToken.None).Result;

            _productRepositoryMock.Verify(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once());
            _productPriceServiceMock.Verify(x => x.CalculateDiscountedPrice(It.IsAny<CampaignEntity>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Once);

            result.Should().NotBeNull();
        }
    }
}
