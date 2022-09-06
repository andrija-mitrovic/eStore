using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    public sealed class CreateProductCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string? PictureUrl { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public int QuantityInStock { get; set; }
    }
}
