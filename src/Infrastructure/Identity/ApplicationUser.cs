using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public UserAddress? Address { get; set; }
    }
}
