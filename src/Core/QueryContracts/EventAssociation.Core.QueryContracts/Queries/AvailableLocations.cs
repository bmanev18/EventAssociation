using EventAssociation.Core.QueryContracts.Contract;

namespace EventAssociation.Core.QueryContracts.Queries;

public abstract class AvailableLocations
{
    public record Query(string startDate, string endDate) : IQuery<EventsOverview.Answer>, IQuery<Answer>;
    
    public record Answer(List<LocationsInfo> Locations);
    public record LocationsInfo(string locationName, string locationType, int maxParticipants, string locationStatus);

}