using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Products.Commands
{
    public sealed class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateProductCommandHandler>> _loggerMock;

        public CreateProductCommandHandlerTests()
        {
            _productRepositoryMock = new();
            _unitOfWorkMock = new();
            _mapperMock = new();
            _loggerMock = new();
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenCreateRequestIsValid()
        {
            //Arrange
            var command = ReturnCreateProductCommand();
            var product = ReturnProduct();

            var handler = new CreateProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
               );

            _mapperMock.Setup(x => x.Map<Product>(command)).Returns(product);

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            result.Should().BePositive();
            result.Should().NotBe(null);
        }

        [Fact]
        public async Task Handle_ShouldCallAddOnRepository_WhenCreateRequestIsValid()
        {
            //Arrange
            var command = ReturnCreateProductCommand();
            var product = ReturnProduct();

            var handler = new CreateProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);

            _mapperMock.Setup(x => x.Map<Product>(command)).Returns(product);

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _productRepositoryMock.Verify(x => x.AddAsync(It.Is<Product>(y => y.Id == result), default), Times.Once);
        }

        private static Product ReturnProduct() =>
            new()
            {
                Id = 1,
                Name = "IPhone",
                Description = "IPhone X",
                Price = 1000,
                PictureUrl = "/iphone.png",
                Type = "Smart Phone",
                Brand = "Apple",
                QuantityInStock = 5
            };

        private static CreateProductCommand ReturnCreateProductCommand() =>
            new()
            {
                Name = "IPhone",
                Description = "IPhone X",
                Price = 1000,
                PictureUrl = "/iphone.png",
                Type = "Smart Phone",
                Brand = "Apple",
                QuantityInStock = 5
            };
    }
}
