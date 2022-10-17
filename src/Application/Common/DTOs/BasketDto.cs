namespace Application.Common.DTOs
{
    public sealed class BasketDto
    {
        public int Id { get; set; }
        public string? BuyerId { get; set; }
        public List<BasketItemDto>? Items { get; set; }
    }
}
