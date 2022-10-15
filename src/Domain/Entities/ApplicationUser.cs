using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public UserAddress? Address { get; set; }
    }
}
