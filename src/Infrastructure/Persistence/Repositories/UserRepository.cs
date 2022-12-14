using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicationUser?> GetByUsername(string username, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}
