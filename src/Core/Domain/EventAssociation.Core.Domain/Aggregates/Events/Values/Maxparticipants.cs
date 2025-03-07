using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class Maxparticipants: ValueObject
{
    public Maxparticipants(int value)
    {
        Value = value;
    }

    private int Value { get;}
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}