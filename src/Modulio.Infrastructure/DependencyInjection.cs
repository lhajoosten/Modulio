using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulio.Application.Abstractions.Services;
using Modulio.Infrastructure.Services;
using Modulio.Persistence;

namespace Modulio.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IAuditService, AuditService>();

            // ⬇️ Worden later toegevoegd
            services.AddPersistence(config);
            services.AddPersistenceHealthChecks(config);
            // services.AddIdentityAccess(config);

            return services;
        }
    }
}