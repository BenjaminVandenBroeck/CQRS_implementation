namespace CQRS_Example.Common.EventStore
{
    public abstract class ValueObject
    {
        public static bool operator ==(ValueObject? first, ValueObject? second)
        {
            if (first == null && second == null) return true;

            if (first == null || second == null) return false;

            return EqualsOperator(first, second);
        }

        public static bool operator !=(ValueObject? first, ValueObject? second)
        {
            if (first == null && second == null) return false;

            if (first == null || second == null) return true;

            return NotEqualsOperator(first, second);
        }

        protected static bool EqualsOperator(ValueObject first, ValueObject second)
        {
            if (ReferenceEquals(first, null) ^ ReferenceEquals(second, null))
            {
                return false;
            }

            return ReferenceEquals(first, second);
        }

        protected static bool NotEqualsOperator(ValueObject first, ValueObject second)
        {
            return !EqualsOperator(first, second);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = obj as ValueObject;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents().Select(x => x != null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
        }
    }
}
