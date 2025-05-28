namespace Modulio.Domain.Base
{
    public class AuditLog : BaseEntity<int>, IAggregateRoot
    {
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string? EntityId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? Details { get; set; }
        public string? Data { get; set; }
        public string Status { get; set; } = "Success";
        public string? ErrorMessage { get; set; }
    }
}