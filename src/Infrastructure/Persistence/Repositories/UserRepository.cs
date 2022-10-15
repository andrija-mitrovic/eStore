using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<ApplicationUser?> GetByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task UpdateUserAddress(string username, ShippingAddress address)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null) throw new NotFoundException($"User with username {username} was not found.");

            user.Address = new UserAddress
            {
                FullName = address.FullName,
                Address1 = address.Address1,
                Address2 = address.Address2,
                City = address.City,
                State = address.State,
                Zip = address.Zip,
                Country = address.Country
            };

            await _dbContext.Users.AddAsync(user);
        }
    }
}
