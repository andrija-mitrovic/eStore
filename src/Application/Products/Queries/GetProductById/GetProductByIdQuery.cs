using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProductById
{
    public sealed class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
