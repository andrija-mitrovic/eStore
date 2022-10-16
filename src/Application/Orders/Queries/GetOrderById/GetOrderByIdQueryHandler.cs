using Application.Common.Interfaces;
using Domain.Entities.OrderAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries.GetOrderById
{
    internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order?>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders.Include(x => x.OrderItems)
                                        .Where(x => x.BuyerId == request.BuyerId && x.Id == request.Id)
                                        .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
