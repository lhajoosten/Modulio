using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Modulio.Persistence.Context;

namespace Modulio.Persistence.HealthChecks
{
    public class DatabaseMigrationHealthCheck : IHealthCheck
    {
        private readonly ModulioDbContext _context;

        public DatabaseMigrationHealthCheck(ModulioDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
                var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync(cancellationToken);

                if (pendingMigrations.Any())
                {
                    return HealthCheckResult.Unhealthy(
                        $"Database has {pendingMigrations.Count()} pending migrations: {string.Join(", ", pendingMigrations)}",
                        data: new Dictionary<string, object>
                        {
                            ["PendingMigrations"] = pendingMigrations.ToArray(),
                            ["AppliedMigrations"] = appliedMigrations.ToArray()
                        });
                }

                return HealthCheckResult.Healthy(
                    $"Database is up to date with {appliedMigrations.Count()} applied migrations",
                    data: new Dictionary<string, object>
                    {
                        ["AppliedMigrations"] = appliedMigrations.ToArray()
                    });
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "Failed to check database migrations",
                    ex);
            }
        }
    }
}