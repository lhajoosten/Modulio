namespace Modulio.Application.Abstractions.Services
{
    /// <summary>
    /// Interface for the audit service
    /// </summary>
    public interface IAuditService
    {
        Task RecordAuditAsync(AuditRecord audit, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents the status of an audited operation
    /// </summary>
    public enum AuditStatus
    {
        Success,
        Failure
    }

    /// <summary>
    /// Represents an audit record
    /// </summary>
    public class AuditRecord
    {
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public int? EntityId { get; set; }
        public string? UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? Details { get; set; }

        // Additional properties used in AuditBehavior
        public string? UserId { get; set; }
        public string? Data { get; set; }
        public AuditStatus Status { get; set; } = AuditStatus.Success;
        public string? ErrorMessage { get; set; }

        public AuditRecord()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static AuditRecord Create(
            string action,
            string entityType,
            int? entityId = null,
            string? userName = null,
            string? ipAddress = null,
            string? details = null,
            string? userId = null,
            string? data = null,
            AuditStatus status = AuditStatus.Success)
        {
            return new AuditRecord
            {
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                UserName = userName,
                IpAddress = ipAddress,
                Details = details,
                UserId = userId,
                Data = data,
                Status = status
            };
        }
    }
}