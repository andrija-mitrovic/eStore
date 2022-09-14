using MediatR;

namespace Application.Baskets.Commands.DeleteBasketItem
{
    public class DeleteBasketItemCommand : IRequest
    {
        public string? BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
