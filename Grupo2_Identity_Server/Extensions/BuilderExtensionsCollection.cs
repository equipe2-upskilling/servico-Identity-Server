using Grupo2_Identity_Server.Context;
using Grupo2_Identity_Server.Interfaces;
using Grupo2_Identity_Server.Repositories;
using Grupo2_Identity_Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Grupo2_Identity_Server.Extensions
{
    public static class BuilderExtensionsCollection
    {
        public static WebApplicationBuilder AddApiSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger();
            return builder;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IdentityServer",
                    Description = "Servidor de Identidade do Grupo 2"
                });
            });
            return services;
        }

        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
            return builder;
        }
       
        public static WebApplicationBuilder AddScoped(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICrypto, CryptoService>();
            builder.Services.AddScoped<IToken, TokenService>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();

            return builder;
        }
    }
}
