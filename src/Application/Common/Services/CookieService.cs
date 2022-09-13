using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Services
{
    public class CookieService : ICookieService
    {
        private const int EXPIRY_DAYS = 30;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddCookie(string key, string value)
        {
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.Now.AddDays(EXPIRY_DAYS)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
        }
    }
}
