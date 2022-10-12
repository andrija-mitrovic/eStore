using Domain.Entities.OrderAggregate;
using MediatR;

namespace Application.Orders.Queries.GetOrders
{
    public sealed class GetOrdersQuery : IRequest<List<Order>>
    {
        public string? BuyerId { get; set; }
    }
}
