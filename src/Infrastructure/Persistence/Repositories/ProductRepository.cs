using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories
{
    internal sealed class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
