using Domain.Entities.OrderAggregate;
using MediatR;

namespace Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommand : IRequest<int>
    {
        public string? Username { get; set; }
        public string? BuyerId { get; set; }
        public bool SaveAddress { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
    }
}
