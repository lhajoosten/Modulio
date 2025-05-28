using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Modulio.Application.Abstractions.Persistence;
using Modulio.Persistence.Context;
using Modulio.Persistence.HealthChecks;
using Modulio.Persistence.Repositories;

namespace Modulio.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<ModulioDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                options.UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly(typeof(ModulioDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
                });

                // Enable sensitive data logging in development
                if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
                {
                    options.EnableSensitiveDataLogging();
                }

                // Enable detailed errors in development
                if (configuration.GetValue<bool>("Logging:EnableDetailedErrors"))
                {
                    options.EnableDetailedErrors();
                }
            });

            // Repositories
            services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddPersistenceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddHealthChecks()
                // Database connectivity
                .AddSqlServer(
                    connectionString,
                    healthQuery: "SELECT 1;",
                    name: "sqlserver",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "database", "sql", "sqlserver" })

                // Database migrations check
                .AddCheck<DatabaseMigrationHealthCheck>(
                    "database-migrations",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "database", "migrations" })

                // Database performance check
                .AddCheck<DatabasePerformanceHealthCheck>(
                    "database-performance",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "database", "performance" })

                // Repository health check
                .AddCheck<RepositoryHealthCheck>(
                    "repositories",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "database", "repositories" });

            return services;
        }

        public static IServiceCollection AddHealthChecksUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(30); // Evaluate every 30 seconds
                options.MaximumHistoryEntriesPerEndpoint(50); // Keep 50 entries per endpoint
                options.AddHealthCheckEndpoint("Modulio API", "/health");
            })
            .AddInMemoryStorage();

            return services;
        }
    }
}