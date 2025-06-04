
using EventAssociation.Core.QueryContracts.Contract;

namespace EventAssociation.Core.QueryContracts.Queries;

public abstract class EventsOverview
{
    public record Query() : IQuery<Answer>;
     
    public record Answer(List<EventInfo> readyEvents, List<EventInfo> draftEvents, List<EventInfo> cancelledEvents);
    public record EventInfo(string Title);
}