using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Modulio.Application.Abstractions.Persistence;
using Modulio.Domain.Base;

namespace Modulio.Persistence.HealthChecks
{
    public class RepositoryHealthCheck : IHealthCheck
    {
        private readonly IQueryRepository<AuditLog> _auditRepository;
        private readonly ILogger<RepositoryHealthCheck> _logger;

        public RepositoryHealthCheck(IQueryRepository<AuditLog> auditRepository, ILogger<RepositoryHealthCheck> logger)
        {
            _auditRepository = auditRepository;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Test basic repository functionality
                var count = await _auditRepository.CountAsync(cancellationToken);

                var data = new Dictionary<string, object>
                {
                    ["AuditLogCount"] = count,
                    ["Timestamp"] = DateTime.UtcNow
                };

                return HealthCheckResult.Healthy(
                    $"Repositories are working correctly. Audit logs: {count}",
                    data: data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository health check failed");
                return HealthCheckResult.Unhealthy(
                    "Repository health check failed",
                    ex);
            }
        }
    }
}