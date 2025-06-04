using EventAssociation.Core.QueryContracts.Queries;
using EventAssociation.Core.QueryContracts.QueryDispatching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObjectMapper;

namespace WebAPI.Endpoints;
using Endpoint = WebAPI.REPRBase.Endpoint;

public class GetEventsOverview(IQueryDispatcher dispatcher, IMapper mapper): Endpoint.WithoutRequest.WithResponse<GetEventsOverviewResponse>
{
    [HttpGet("events")]
    public override async Task<ActionResult<GetEventsOverviewResponse>> HandleAsync()
    {
        EventsOverview.Query query = new EventsOverview.Query();
        EventsOverview.Answer answer = await dispatcher.DispatchAsync(query);
        GetEventsOverviewResponse response = mapper.Map<GetEventsOverviewResponse>(answer);
        return Ok(response);
    }
}


public record GetEventsOverviewResponse(
    IEnumerable<GetEventsOverviewResponse.EventInfo> ReadyEvents,
    IEnumerable<GetEventsOverviewResponse.EventInfo> DraftEvents,
    IEnumerable<GetEventsOverviewResponse.EventInfo> CancelledEvents
)

{
    public record EventInfo(string Title);
}
