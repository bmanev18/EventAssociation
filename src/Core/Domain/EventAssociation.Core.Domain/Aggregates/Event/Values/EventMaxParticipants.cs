using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventMaxParticipants: ValueObject
{
    public EventMaxParticipants(int value)
    {
        Value = value;
    }

    private int Value { get;}
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}