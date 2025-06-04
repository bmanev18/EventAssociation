using EventAssociation.Core.QueryContracts.Queries;
using ObjectMapper;
using WebAPI.Endpoints;

namespace WebAPI.Mappers;

public class GetEventsOverviewResponseMapping 
    : IMappingConfig<GetEventsOverviewResponse, EventsOverview.Answer>
{
    public EventsOverview.Answer Map(GetEventsOverviewResponse input)
    {
        return new EventsOverview.Answer(
            readyEvents: input.ReadyEvents.Select(e => new EventsOverview.EventInfo(e.Title)).ToList(),
            draftEvents: input.DraftEvents.Select(e => new EventsOverview.EventInfo(e.Title)).ToList(),
            cancelledEvents: input.CancelledEvents.Select(e => new EventsOverview.EventInfo(e.Title)).ToList()
        );
    }

}