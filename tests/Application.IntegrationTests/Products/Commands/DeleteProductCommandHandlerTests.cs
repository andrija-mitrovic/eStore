using API.Services;
using Application.Common.Interfaces;
using Application.Products.Commands.DeleteProduct;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.Products.Commands
{
    public sealed class DeleteProductCommandHandlerTests
    {
        private const string APPSETTINGS_FILE = "appsettings.json";
        private const string CONNECTION_STRING = "DefaultConnection";

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandlerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(APPSETTINGS_FILE, true, true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(configuration.GetConnectionString(CONNECTION_STRING));

            var currentUserService = new CurrentUserService(new HttpContextAccessor());
            var interceptor = new AuditableEntitySaveChangesInterceptor(currentUserService, new DateTimeService());
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options, interceptor);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _productRepository = new ProductRepository(applicationDbContext);
            _unitOfWork = new UnitOfWork(applicationDbContext);
            _logger = factory!.CreateLogger<DeleteProductCommandHandler>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenDeleteRequestIsValid()
        {
            var command = await ReturnDeleteProductCommand();

            var handler = new DeleteProductCommandHandler(
                _productRepository,
                _unitOfWork,
                _logger);

            var result = await handler.Handle(command, default);

            result.Should().NotBeNull();
        }

        private async Task<DeleteProductCommand> ReturnDeleteProductCommand() =>
            new()
            {
                Id = await GetProductId() ?? 0
            };

        private async Task<int?> GetProductId()
        {
            return (await _productRepository.GetAsync(x => x.Name == ReturnProduct().Name &&
                                                           x.Description == ReturnProduct().Description &&
                                                           x.Price == ReturnProduct().Price &&
                                                           x.PictureUrl == ReturnProduct().PictureUrl &&
                                                           x.Type == ReturnProduct().Type &&
                                                           x.Brand == ReturnProduct().Brand &&
                                                           x.QuantityInStock == ReturnProduct().QuantityInStock))
                                             .FirstOrDefault()?.Id;
        }

        private static Product ReturnProduct() =>
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
