using Domain.Entities.OrderAggregate;
using MediatR;

namespace Application.Orders.Queries.GetOrderById
{
    public sealed class GetOrderByIdQuery : IRequest<Order?>
    {
        public int Id { get; set; }
        public string? BuyerId { get; set; }
    }
}
