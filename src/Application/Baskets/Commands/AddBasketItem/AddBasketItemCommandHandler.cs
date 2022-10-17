using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Commands.AddBasketItem
{
    internal sealed class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, BasketDto?>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICookieService _cookieService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AddBasketItemCommandHandler> _logger;

        public AddBasketItemCommandHandler(
            IBasketRepository basketRepository, 
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ICookieService cookieService, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<AddBasketItemCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _cookieService = cookieService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<BasketDto?> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.BuyerId))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieConstants.KEY);
            }

            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!, cancellationToken);

            if (basket == null) basket = await CreateBasket(cancellationToken);

            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - " + nameof(Product) + " with Id: {ProductId} was not found.", request.ProductId);
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            basket.AddItem(product, request.Quantity);

            var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem saving item to basket.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return _mapper.Map<BasketDto>(basket);
        }

        private async Task<Basket> CreateBasket(CancellationToken cancellationToken)
        {
            var buyerId = _httpContextAccessor.HttpContext.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                _cookieService.AddCookie(CookieConstants.KEY, buyerId);
            }

            var basket = new Basket { BuyerId = buyerId };

            await _basketRepository.AddAsync(basket, cancellationToken);

            return basket;
        }
    }
}
