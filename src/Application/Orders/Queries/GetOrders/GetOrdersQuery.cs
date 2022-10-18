using Application.Common.DTOs;
using MediatR;

namespace Application.Orders.Queries.GetOrders
{
    public sealed class GetOrdersQuery : IRequest<List<OrderDto>>
    {
        public string? BuyerId { get; set; }
    }
}
