using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Locations.Values;

public class LocationCapacity: ValueObject
{
    public int Value { get; private set; }

    private LocationCapacity(int value)
    {
        Value = value;
    }

    public static Result<LocationCapacity> CreateLocationCapacity(int value)
    {
        var result = new LocationCapacity(value);
        return Result<LocationCapacity>.Ok(result);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}