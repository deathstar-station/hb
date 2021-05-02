using Bogus;
using FluentAssertions;
using Hepsiburada.Application.Infrastructure;
using Hepsiburada.Application.Order.Commands.CreateOrder;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Campaign;
using Hepsiburada.Domain.Order;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateOrder
{
    public class CreateOrderCommandTests
    {
        private readonly Mock<IRepository<CampaignEntity>> _campaignRepositoryMock;
        private readonly Mock<IRepository<ProductEntity>> _productRepositoryMock;
        private readonly Mock<IRepository<OrderEntity>> _orderRepositoryMock;
        private readonly Mock<ICreateOrderService> _createOrderServiceMock;
        private readonly CreateOrderHandler _handler;

        private readonly Faker<CreateOrderCommand> _createOrderFaker;

        public CreateOrderCommandTests()
        {
            _campaignRepositoryMock = new Mock<IRepository<CampaignEntity>>();
            _productRepositoryMock = new Mock<IRepository<ProductEntity>>();
            _orderRepositoryMock = new Mock<IRepository<OrderEntity>>();
            var timeServiceMock = new Mock<ITimeService>();
            _createOrderServiceMock = new Mock<ICreateOrderService>();

            _createOrderFaker = new Faker<CreateOrderCommand>()
                .RuleFor(x => x.ProductCode, f => f.Random.AlphaNumeric(3))
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10));

            _handler= new(
                _orderRepositoryMock.Object,
                _productRepositoryMock.Object,
                _campaignRepositoryMock.Object,
                _createOrderServiceMock.Object,
                timeServiceMock.Object);
        }

        
        [Fact]
        public void CreateOrder_Command_ShouldBe_Throw_Exception_When_ProductNotExists()
        {
            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(default(ProductEntity));

            Assert.ThrowsAsync<ProductNotFoundException>(async () =>
            {
                await _handler.Handle(_createOrderFaker, CancellationToken.None);
            }).Wait();
        }

        [Fact]
        public void CreateOrder_Command_Should_Run_Correctly_If_Product_Have_Campaign()
        {
            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(FakeObjects.GenerateProduct());
            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProductEntity>()))
                .Returns(Task.CompletedTask);

            var campaign = FakeObjects.GenerateCampaign();

            ((IEntityBase)campaign).SetId(1234);

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity> { campaign });

            _campaignRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CampaignEntity>()))
                .Returns(Task.CompletedTask);

            var order = FakeObjects.GenerateOrder();
            _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<OrderEntity>())).
                    ReturnsAsync(order);

            _createOrderServiceMock.Setup(x =>
                    x.Create(It.IsAny<ProductEntity>(), It.IsAny<int>(), It.IsAny<CampaignEntity>(), It.IsAny<int>()))
                .Returns(order);

            var result = _handler.Handle(_createOrderFaker.Generate(), CancellationToken.None).Result;

            _productRepositoryMock.Verify(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProductEntity>()), Times.Once);

            _campaignRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CampaignEntity>()), Times.Once);

            _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OrderEntity>()), Times.Once);

            result.Should().NotBeNull();
        }

        [Fact]
        public void CreateOrder_Command_Should_Run_Correctly_If_Product_DoesNot_Have_Campaign()
        {
            _productRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(FakeObjects.GenerateProduct());
            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProductEntity>()))
                .Returns(Task.CompletedTask);

            _campaignRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()))
                .ReturnsAsync(new List<CampaignEntity>());
            _campaignRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CampaignEntity>()))
                .Returns(Task.CompletedTask);

            _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<OrderEntity>())).
                    ReturnsAsync(FakeObjects.GenerateOrder());

            var result = _handler.Handle(_createOrderFaker.Generate(), CancellationToken.None).Result;

            _productRepositoryMock.Verify(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProductEntity>()), Times.Once);

            _campaignRepositoryMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<CampaignEntity, bool>>>()), Times.Once);
            _campaignRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CampaignEntity>()), Times.Never);

            _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OrderEntity>()), Times.Once);

            result.Should().NotBeNull();
        }
    }
}
