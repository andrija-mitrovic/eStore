using Application.Common.Constants;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public sealed class JwtTokenService : ITokenService
    {
        private const int EXPIRY_DAYS = 7;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(string userName, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            List<Claim> claims = GenerateClaims(userName, email);

            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            SecurityTokenDescriptor tokenDescriptor = GenerateTokenDescription(claims, creds);

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private List<Claim> GenerateClaims(string userName, string email)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName),
            };
        }

        private SecurityTokenDescriptor GenerateTokenDescription(List<Claim> claims, SigningCredentials creds)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(EXPIRY_DAYS),
                SigningCredentials = creds
            };
        }
    }
}
