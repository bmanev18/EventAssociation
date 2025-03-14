using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Locations;

public class Location: AggregateRoot
{
    internal LocationId Id { get; private set; }
    internal LocationType LocationType { get; private set; }
    internal LocationName LocationName { get; private set; }
    internal LocationCapacity LocationCapacity { get; private set; }

    private Location(LocationType locationType, LocationName locationName, LocationCapacity locationCapacity)
    {
        Id = new LocationId(Guid.NewGuid());
        LocationType = locationType;
        LocationName = locationName;
        LocationCapacity = locationCapacity;
    }

    public static Result<Location> CreateLocation(LocationType locationType, LocationName locationName, LocationCapacity locationCapacity)
    {
        var result = new Location(locationType, locationName, locationCapacity);
        return Result<Location>.Ok(result);
    }
    
    
}