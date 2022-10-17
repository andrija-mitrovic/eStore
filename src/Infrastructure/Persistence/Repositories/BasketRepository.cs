using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    internal sealed class BasketRepository : GenericRepository<Basket>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Basket?> GetBasketByBuyerId(string buyerId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Baskets.Include(x => x.Items)
                                           .ThenInclude(x => x.Product)
                                           .FirstOrDefaultAsync(x => x.BuyerId == buyerId, cancellationToken);
        }
    }
}
