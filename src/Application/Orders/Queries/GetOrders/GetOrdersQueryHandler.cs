using Application.Common.Interfaces;
using Domain.Entities.OrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries.GetOrders
{
    internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
    {
        private readonly IApplicationDbContext _context;

        public GetOrdersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders.Include(x=>x.OrderItems)
                                        .Where(x=>x.BuyerId == request.BuyerId)
                                        .ToListAsync();
        }
    }
}
