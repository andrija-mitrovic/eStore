using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByUsername(string username, CancellationToken cancellationToken = default);
    }
}
