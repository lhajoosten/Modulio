using Microsoft.Extensions.Logging;
using Modulio.Application.Abstractions.Services;
using Modulio.Domain.Base;
using Modulio.Persistence.Context;

namespace Modulio.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly ModulioDbContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(ModulioDbContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RecordAuditAsync(AuditRecord audit, CancellationToken cancellationToken = default)
        {
            try
            {
                var auditEntity = new AuditLog
                {
                    Action = audit.Action,
                    EntityType = audit.EntityType,
                    EntityId = audit.EntityId?.ToString(),
                    UserId = audit.UserId,
                    UserName = audit.UserName,
                    Timestamp = audit.Timestamp,
                    IpAddress = audit.IpAddress,
                    Details = audit.Details,
                    Data = audit.Data,
                    Status = audit.Status.ToString(),
                    ErrorMessage = audit.ErrorMessage
                };

                _context.AuditLogs.Add(auditEntity);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Audit record created: {Action} on {EntityType} by {UserName}",
                    audit.Action, audit.EntityType, audit.UserName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to record audit: {Action} on {EntityType}",
                    audit.Action, audit.EntityType);
            }
        }
    }
}