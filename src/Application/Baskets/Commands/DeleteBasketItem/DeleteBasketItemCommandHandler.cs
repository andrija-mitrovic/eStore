using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Commands.DeleteBasketItem
{
    internal sealed class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommand>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBasketItemCommandHandler> _logger;

        public DeleteBasketItemCommandHandler(
            IBasketRepository basketRepository, 
            IProductRepository productRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<DeleteBasketItemCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.BuyerId))
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - " + nameof(request.BuyerId) + " can't be null or empty.", request.BuyerId);
                throw new BuyerIdNullException($"{nameof(request.BuyerId)} is null or empty");
            }

            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!, cancellationToken);

            if (basket == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - " + nameof(Basket) + " with " + nameof(request.BuyerId) + ": {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException(nameof(Basket), request.ProductId);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - " + nameof(Product) + " with Id: {ProductId} was not found.", request.ProductId);
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            basket.RemoveItem(request.ProductId, request.Quantity);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem removing item from the basket.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return Unit.Value;
        }
    }
}
