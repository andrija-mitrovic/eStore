using Domain.Entities;
using Domain.Entities.OrderAggregate;

namespace Application.Common.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task UpdateUserAddress(string username, ShippingAddress address);
        Task<ApplicationUser?> GetByUsername(string username);
    }
}
