using Domain.Common;

namespace Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int Quantity { get; set; }
        public Basket? Basket { get; set; }
        public int BasketId { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
    }
}