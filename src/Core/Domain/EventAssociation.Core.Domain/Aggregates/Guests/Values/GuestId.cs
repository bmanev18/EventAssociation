using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class GuestId: ValueObject
{
    private Guid Value { get; }
    
    public GuestId(Guid value)
    {
        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}