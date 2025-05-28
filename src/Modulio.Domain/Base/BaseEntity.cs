namespace Modulio.Domain.Base
{
    public abstract class BaseEntity<TId> : IAuditableEntity where TId : struct, IEquatable<TId>
    {
        // Identity - make setter protected
        public TId Id { get; protected set; }

        // Audit properties
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        protected BaseEntity(TId id) : this()
        {
            Id = id;
        }

        // Identity-based equality
        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            var other = (BaseEntity<TId>)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(BaseEntity<TId>? left, BaseEntity<TId>? right) => !(left == right);
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
        protected BaseEntity() : base(Guid.NewGuid()) { }
        protected BaseEntity(Guid id) : base(id) { }
    }
}
