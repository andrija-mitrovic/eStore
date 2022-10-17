using Application.Common.Interfaces;
using Domain.Entities.OrderAggregate;

namespace Infrastructure.Persistence.Repositories
{
    internal sealed class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
