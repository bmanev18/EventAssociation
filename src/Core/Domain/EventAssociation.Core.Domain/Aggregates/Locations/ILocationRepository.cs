using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Locations;

public interface ILocationRepository
{
    public Task<Result<Location>> CreateAsync(Location location);
    public Task<Result<Location>> GetAsync(LocationId locationId);
    public Task<Result<None>> RemoveAsync(LocationId locationId);
}