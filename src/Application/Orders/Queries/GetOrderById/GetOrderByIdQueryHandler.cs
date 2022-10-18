using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities.OrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrderById
{
    internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderByIdQueryHandler> _logger;

        public GetOrderByIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            ILogger<GetOrderByIdQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Include(x => x.OrderItems)
                                             .AsNoTracking()
                                             .Where(x => x.BuyerId == request.BuyerId && x.Id == request.Id)
                                             .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                _logger.LogError(nameof(Order) + " with Id: {OrderId} was not found.", request.Id);
                throw new NotFoundException(nameof(Order), request.Id);
            }

            return _mapper.Map<OrderDto>(order);
        }
    }
}
