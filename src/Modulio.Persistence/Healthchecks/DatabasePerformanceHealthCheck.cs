using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Modulio.Persistence.Context;
using System.Diagnostics;

namespace Modulio.Persistence.HealthChecks
{
    public class DatabasePerformanceHealthCheck : IHealthCheck
    {
        private readonly ModulioDbContext _context;
        private const int SlowQueryThresholdMs = 1000;
        private const int VerySlowQueryThresholdMs = 5000;

        public DatabasePerformanceHealthCheck(ModulioDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Simple query to test database response time
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);

                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;

                var data = new Dictionary<string, object>
                {
                    ["ResponseTimeMs"] = responseTime,
                    ["Timestamp"] = DateTime.UtcNow
                };

                if (responseTime > VerySlowQueryThresholdMs)
                {
                    return HealthCheckResult.Unhealthy(
                        $"Database response time is very slow: {responseTime}ms",
                        data: data);
                }

                if (responseTime > SlowQueryThresholdMs)
                {
                    return HealthCheckResult.Degraded(
                        $"Database response time is slow: {responseTime}ms",
                        data: data);
                }

                return HealthCheckResult.Healthy(
                    $"Database response time is good: {responseTime}ms",
                    data: data);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "Failed to check database performance",
                    ex);
            }
        }
    }
}