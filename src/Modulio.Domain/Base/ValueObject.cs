namespace Modulio.Domain.Base
{
    /// <summary>
    /// Base class for Value Objects in Domain-Driven Design.
    /// Value objects are immutable and are compared by their structural equality.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Gets the atomic values of this value object.
        /// </summary>
        /// <returns>The atomic values that make up this value object.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Creates a copy of the current value object with some properties modified.
        /// </summary>
        /// <typeparam name="T">The type of value object being copied.</typeparam>
        /// <param name="self">The object to copy.</param>
        /// <param name="modifier">The modification function to apply.</param>
        /// <returns>A new instance with the modifications applied.</returns>
        protected static T With<T>(T self, Action<T> modifier) where T : ValueObject, new()
        {
            var copy = new T();
            modifier(copy);
            return copy;
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
    }
}
