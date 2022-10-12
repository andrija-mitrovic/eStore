using Domain.Entities.OrderAggregate;

namespace Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task UpdateUserAddress(string username, ShippingAddress address);
    }
}
