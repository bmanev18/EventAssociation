using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes
{
    public class FakeLocationRepository : ILocationRepository
    {
        private readonly List<Location> _locations;

        public FakeLocationRepository()
        {
            _locations = new List<Location>();

            // First Location
            var location1 = Location.CreateLocation(
                LocationType.Outside,
                LocationName.Create("Conference Hall").Unwrap(),
                LocationCapacity.Create(500).Unwrap()
            ).Unwrap();
            _locations.Add(location1);

            // Second Location
            var location2 = Location.CreateLocation(
                LocationType.Outside,
                LocationName.Create("Meadows").Unwrap(),
                LocationCapacity.Create(2000).Unwrap()
            ).Unwrap();
            _locations.Add(location2);

            // Third Location
            var location3 = Location.CreateLocation(
                LocationType.Room,
                LocationName.Create("Co-working Space").Unwrap(),
                LocationCapacity.Create(100).Unwrap()
            ).Unwrap();
            _locations.Add(location3);
        }

        public Task<Result<Location>> CreateAsync(Location location)
        {
            _locations.Add(location);
            return Task.FromResult(Result<Location>.Ok(location));
        }

        public Task<Result<Location>> GetAsync(LocationId locationId)
        {
            var location = _locations.FirstOrDefault(l => l.Id.Equals(locationId));
            return location != null
                ? Task.FromResult(Result<Location>.Ok(location))
                : Task.FromResult(Result<Location>.Err(new Error("", "Location not found")));
        }

        public Task<Result<None>> RemoveAsync(LocationId locationId)
        {
            var location = _locations.FirstOrDefault(l => l.Id.Equals(locationId));
            if (location != null)
            {
                _locations.Remove(location);
                return Task.FromResult(Result<None>.Ok(None.Value));
            }
            return Task.FromResult(Result<None>.Err(new Error("", "Location not found")));
        }
    }
}
