using System;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hastnama.Solico.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SolicoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SolicoContext"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        10,
                        TimeSpan.FromSeconds(30),
                        null);
                });
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<ISolicoDbContext>(provider => provider.GetService<SolicoContext>());

            return services;
        }
    }
}