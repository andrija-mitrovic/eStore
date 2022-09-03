using API.Services;
using Application.Common.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static void AddAPIServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwagger();
            services.AddServices();
            services.AddHttpContext();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        }

        private static void AddHttpContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
