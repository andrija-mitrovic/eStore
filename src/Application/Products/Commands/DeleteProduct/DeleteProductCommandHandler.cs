using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.DeleteProduct
{
    internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(
            IProductRepository productRepository, 
            IUnitOfWork unitOfWork,
            ILogger<DeleteProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;   
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogError(nameof(Product) + " with Id: {ProductId} was not found.", request.Id);
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _productRepository.Delete(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(nameof(product) + " with Id: {ProductId} is successfully deleted.", product.Id);

            return Unit.Value;
        }
    }
}
