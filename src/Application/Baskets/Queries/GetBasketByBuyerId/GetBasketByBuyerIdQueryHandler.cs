using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Queries.GetBasketByBuyerId
{
    internal sealed class GetBasketByBuyerIdQueryHandler : IRequestHandler<GetBasketByBuyerIdQuery, BasketDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBasketByBuyerIdQueryHandler> _logger;

        public GetBasketByBuyerIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper, 
            ILogger<GetBasketByBuyerIdQueryHandler> logger)
        {
            _context = context;
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

            var basket = await _context.Baskets.Include(x => x.Items)
                                               .ThenInclude(x => x.Product)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(x => x.BuyerId == request.BuyerId);

            if (basket == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Basket with BuyerId: {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException($"{nameof(Basket)} with Buyer Id: {request.BuyerId} was not found.");
            }

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
