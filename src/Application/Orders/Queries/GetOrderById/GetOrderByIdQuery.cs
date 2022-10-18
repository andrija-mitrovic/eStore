using Application.Common.DTOs;
using MediatR;

namespace Application.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdQuery : IRequest<OrderDto?>
    {
        public int Id { get; set; }
        public string? BuyerId { get; set; }
    }
}
