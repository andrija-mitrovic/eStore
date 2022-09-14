using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Queries.GetBasketByBuyerId
{
    public class GetBasketByBuyerIdQueryHandler : IRequestHandler<GetBasketByBuyerIdQuery, BasketDto>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBasketByBuyerIdQueryHandler> _logger;

        public GetBasketByBuyerIdQueryHandler(
            IBasketRepository basketRepository, 
            IMapper mapper, 
            ILogger<GetBasketByBuyerIdQueryHandler> logger)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BasketDto> Handle(GetBasketByBuyerIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.BuyerId))
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - " + nameof(request.BuyerId) + " can't be is null or empty.");
                throw new BuyerIdNullException($"{nameof(request.BuyerId)} is null or empty");
            }

            var basket = await _basketRepository.GetBasketByBuyerId(request.BuyerId!);

            if (basket == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Basket with BuyerId: {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException(nameof(Basket), request.BuyerId!);
            }

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
