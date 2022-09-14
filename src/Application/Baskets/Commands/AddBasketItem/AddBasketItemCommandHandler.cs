﻿using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Commands.AddBasketItem
{
    public class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, BasketDto>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICookieService _cookieService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBasketItemCommandHandler> _logger;

        public AddBasketItemCommandHandler(
            IBasketRepository basketRepository, 
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ICookieService cookieService, 
            IMapper mapper, 
            ILogger<AddBasketItemCommandHandler> logger)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _cookieService = cookieService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BasketDto> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!);

            if (basket == null)
            {
                basket = await CreateBasket();
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);

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

        private async Task<Basket> CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();

            _cookieService.AddCookie(CookieConstants.KEY, buyerId);

            var basket = new Basket { BuyerId = buyerId };

            await _basketRepository.AddAsync(basket);

            return basket;
        }
    }
}