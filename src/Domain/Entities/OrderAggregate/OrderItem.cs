namespace Domain.Entities.OrderAggregate
{
    public sealed class OrderItem
    {
        public int Id { get; set; }
        public ProductItemOrdered? ItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
