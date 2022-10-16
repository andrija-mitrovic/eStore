using Application.Common.DTOs;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Products.Queries.GetProductsWithPagination
{
    internal sealed class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProductsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products
                                         .Sort(request.OrderBy)
                                         .Search(request.SearchTerm)
                                         .Filter(request.Brands, request.Types)
                                         .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                                         .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(request.PageNumber, request.PageSize, products.TotalCount, products.TotalPages);

            return products;
        }
    }
}
