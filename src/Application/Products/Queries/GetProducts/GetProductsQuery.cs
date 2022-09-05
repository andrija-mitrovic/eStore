using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    public sealed class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
