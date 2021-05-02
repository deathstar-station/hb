using FluentAssertions;
using Hepsiburada.Application.Product.Commands.CreateProduct;
using Hepsiburada.Application.Product.Exceptions;
using Hepsiburada.Domain.Product;
using Hepsiburada.Domain.Shared.Abstractions;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using Bogus;
using Xunit;

namespace Hepsiburada.Tests.Application.CreateProduct
{
    public class CreateProductCommandTests
    {
        private readonly Mock<IRepository<ProductEntity>> _productRepositoryMock;
        private readonly CreateProductHandler _handler;
        private readonly Faker<CreateProductCommand> _createProductFaker;

        public CreateProductCommandTests()
        {
            _productRepositoryMock = new Mock<IRepository<ProductEntity>>();
            _handler = new(_productRepositoryMock.Object);

            _createProductFaker = new Faker<CreateProductCommand>()
                .RuleFor(x => x.ProductCode, f => f.Random.AlphaNumeric(3))
                .RuleFor(x => x.Price, f => f.Random.Decimal(10, 100))
                .RuleFor(x => x.Stock, f => f.Random.Int(1, 20));
        }

        [Fact]
        public void CreateProduct_Command_ShouldBe_Throw_Exception_When_ProductAlreadyExists()
        {
            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<ProductAlreadyExistsException>(async () =>
            {
                await _handler.Handle(_createProductFaker.Generate(), CancellationToken.None);
            }).Wait();
        }

        [Fact]
        public void CreateProduct_Command_Should_Return_Repository_Product()
        {
            var product = FakeObjects.GenerateProduct();

            _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ProductEntity>()))
                .ReturnsAsync(product);

            var response = _handler.Handle(_createProductFaker.Generate(), CancellationToken.None);

            response.Result.Product.Should().Be(product);
        }

        [Fact]
        public void CreateProduct_Command_Should_Run_Correctly()
        {
            var product = FakeObjects.GenerateProduct();

            _productRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                .ReturnsAsync(false);

            _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ProductEntity>()))
                .ReturnsAsync(product);

            var result = _handler.Handle(_createProductFaker.Generate(), CancellationToken.None).Result;

            _productRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<ProductEntity, bool>>>()), Times.Once);
            _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ProductEntity>()), Times.Once);

            result.Should().NotBeNull();
        }
    }
}
