using Domain.Entities.OrderAggregate;

namespace Application.Common.DTOs
{
    public sealed class CreateOrder
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
    }
}
