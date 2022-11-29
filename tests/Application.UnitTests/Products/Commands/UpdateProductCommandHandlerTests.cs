using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Products.Commands.UpdateProduct;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Products.Commands
{
    public sealed class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateProductCommandHandler>> _loggerMock;

        public UpdateProductCommandHandlerTests()
        {
            _productRepositoryMock = new();
            _unitOfWorkMock = new();
            _mapperMock = new();
            _loggerMock = new();
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenUpdateRequestIsValid()
        {
            //Arrange
            var command = ReturnUpdateProductCommand();
            var product = ReturnProduct();

            var handler = new UpdateProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default)).ReturnsAsync(product);

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesntExist()
        {
            //Arrange
            var command = ReturnUpdateProductCommand();

            var handler = new UpdateProductCommandHandler(
                _productRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), default)).ReturnsAsync(value: null);

            //Act
            Func<Task> result = async () => await handler.Handle(command, default);

            //Assert
            await result.Should().ThrowAsync<NotFoundException>();
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

        private static UpdateProductCommand ReturnUpdateProductCommand() =>
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
    }
}
