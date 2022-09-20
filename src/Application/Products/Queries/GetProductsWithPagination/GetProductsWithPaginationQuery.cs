using Application.Common.DTOs;
using Application.Common.Models;
using MediatR;

namespace Application.Products.Queries.GetProductsWithPagination
{
    public sealed class GetProductsWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? Types { get; set; }
        public string? Brands { get; set; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
