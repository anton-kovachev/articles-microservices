using System.Numerics;

namespace Blocks.Domain.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object?> GetEqualityComponents(); 

    public bool Equals(ValueObject? other)
    {
        if (other == null || other.GetType() != GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ValueObject);
    }

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));

    public static bool operator == (ValueObject? a, ValueObject? b) => Equals(a, b);

    public static bool operator != (ValueObject? a, ValueObject? b) => !Equals(a, b);
}
