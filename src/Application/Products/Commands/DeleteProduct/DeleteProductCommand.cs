using MediatR;

namespace Application.Products.Commands.DeleteProduct
{
    public sealed class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}
