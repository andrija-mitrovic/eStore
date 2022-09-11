using Domain.Common;

namespace Domain.Entities
{
    public class Basket : BaseEntity
    {
        public string? BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();
    }
}
