using API.Services;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Products.Commands.CreateProduct;
using AutoMapper;
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
using System.Threading.Tasks;
using Xunit;

namespace Application.IntegrationTests.Products.Commands
{
    public sealed class CreateProductCommandHandlerTests
    {
        private const string APPSETTINGS_FILE = "appsettings.json";
        private const string CONNECTION_STRING = "DefaultConnection";

        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandlerTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(APPSETTINGS_FILE, true, true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(configuration.GetConnectionString(CONNECTION_STRING));

            var currentUserService = new CurrentUserService(new HttpContextAccessor());
            var interceptor = new AuditableEntitySaveChangesInterceptor(currentUserService, new DateTimeService());
            var applicationDbContext = new ApplicationDbContext(optionsBuilder.Options, interceptor);

            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _productRepository = new ProductRepository(applicationDbContext);
            _unitOfWork = new UnitOfWork(applicationDbContext);
            _mapper = mapperConfiguration.CreateMapper();
            _logger = factory!.CreateLogger<CreateProductCommandHandler>();
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenCreateRequestIsValid()
        {
            var command = ReturnCreateProductCommand();

            var handler = new CreateProductCommandHandler(
                _productRepository,
                _unitOfWork,
                _mapper,
                _logger);

            var result = await handler.Handle(command, default);

            result.Should().BePositive();
            result.Should().NotBe(null);
        }

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
