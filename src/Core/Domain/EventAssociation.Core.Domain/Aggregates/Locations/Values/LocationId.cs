using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Locations.Values;

public class LocationId : ValueObject
{
    public Guid Value { get; }
    
    private LocationId() { }
    private LocationId(Guid guid) => Value = guid;
    
    public static LocationId Create() => new LocationId(Guid.NewGuid());
    
    public static LocationId FromGuid(Guid guid) => new LocationId(guid);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}