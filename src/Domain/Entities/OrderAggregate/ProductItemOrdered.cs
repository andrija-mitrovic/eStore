namespace Domain.Entities.OrderAggregate
{
    public sealed class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
    }
}
