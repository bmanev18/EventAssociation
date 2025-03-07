using System.Diagnostics;

namespace EventAssociation.Core.Domain.Common.Bases;

public abstract class EntityObject<T>
{
    public T Id { get; private set; }

    protected EntityObject(T id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;
        var other = (EntityObject<T>)obj;
        return Id != null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityObject<T> left, EntityObject<T> right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(EntityObject<T> left, EntityObject<T> right)
    {
        return !(left == right);
    }
}