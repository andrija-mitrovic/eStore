using API.Filters;
using API.Services;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        private const string SWAGGER_BEARER = "Bearer";
        private const string SWAGGER_SCHEME = "oauth2";
        private const string SWAGGER_AUTHORIZATION = "Authorization";
        private const string SWAGGER_DESCRIPTION = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                                     Enter 'Bearer' [space] and then your token in the text input below.
                                                     \r\n\r\nExample: 'Bearer 12345abcdef'";

        public static void AddAPIServices(this IServiceCollection services)
        {
            services.AddControllers(opt => opt.Filters.Add<ApiExceptionFilterAttribute>());
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddServices();
            services.AddHttpContext();
            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
            services.AddCors();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
        }

        private static void AddHttpContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        private static void AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                option.AddSecurityDefinition(SWAGGER_BEARER, GenerateOpenApiSecurityScheme());
                option.AddSecurityRequirement(GenerateOpenApiSecurityRequirement());
            });
        }

        private static OpenApiSecurityRequirement GenerateOpenApiSecurityRequirement()
        {
            return new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SWAGGER_BEARER
                        },
                        Scheme = SWAGGER_SCHEME,
                        Name = SWAGGER_BEARER,
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            };
        }

        private static OpenApiSecurityScheme GenerateOpenApiSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Description = SWAGGER_DESCRIPTION,
                Name = SWAGGER_AUTHORIZATION,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SWAGGER_BEARER
            };
        }
    }
}
