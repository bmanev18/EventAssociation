using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Locations.Values;

public class LocationName : ValueObject
{
    private string Value { get; }


    private LocationName(string value)
    {
        Value = value;
    }

    public static Result<LocationName> Create(string locationName)
    {
        var result = new LocationName(locationName);
        return Result<LocationName>.Ok(result);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}