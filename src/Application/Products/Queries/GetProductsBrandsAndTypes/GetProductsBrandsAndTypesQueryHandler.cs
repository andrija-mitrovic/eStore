using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.GetProductsBrandsAndTypes
{
    internal sealed class GetProductsBrandsAndTypesQueryHandler : IRequestHandler<GetProductsBrandsAndTypesQuery, object>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsBrandsAndTypesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(GetProductsBrandsAndTypesQuery request, CancellationToken cancellationToken)
        {
            var brands = await _context.Products.Select(x => x.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(x => x.Type).Distinct().ToListAsync();

            return new { brands, types };
        }
    }
}
