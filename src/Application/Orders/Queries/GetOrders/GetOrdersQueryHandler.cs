using Application.Common.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries.GetOrders
{
    internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x=>x.OrderItems)
                                              .AsNoTracking()
                                              .Where(x=>x.BuyerId == request.BuyerId)
                                              .ToListAsync(cancellationToken);

            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
