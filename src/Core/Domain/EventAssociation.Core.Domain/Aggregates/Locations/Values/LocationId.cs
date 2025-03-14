using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Locations.Values;

public class LocationId : ValueObject
{
    private Guid Value { get; }
    
    public LocationId(Guid value)
    {
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}