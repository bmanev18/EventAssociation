using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Locations.Values;

public class LocationTime: ValueObject
{
    private DateTime Value { get; }
    
    public LocationTime(DateTime value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}