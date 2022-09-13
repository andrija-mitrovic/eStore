using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        public Task<Basket?> GetBasketByBuyerId(string buyerId);
    }
}
