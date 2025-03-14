using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventTime: ValueObject
{
    public EventTime(DateTime value)
    {
        Value = value;
    }

    private DateTime Value { get; }
    
    

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}