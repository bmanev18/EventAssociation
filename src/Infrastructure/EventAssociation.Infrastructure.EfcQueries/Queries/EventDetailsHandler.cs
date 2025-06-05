using EventAssociation.Core.QueryContracts.Contract;
using EventAssociation.Core.QueryContracts.Queries;
using EventAssociation.Infrastructure.EfcQueries.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.EfcQueries.Queries;

public class EventDetailsHandler(EventAssociationProductionContext context) : IQueryHandler<EventsOverview.Query, EventsOverview.Answer>
{
    public async Task<EventsOverview.Answer> HandleAsync(EventsOverview.Query query)
    {
        List<EventsOverview.EventInfo> draftEvents = await context.Events
            .Where(e => e.Status.ToLower() == "draft")
            .Select(e => new EventsOverview.EventInfo(e.Title))
            .ToListAsync();

        List<EventsOverview.EventInfo> readyEvents = await context.Events
            .Where(e => e.Status.ToLower() == "ready")
            .Select(e => new EventsOverview.EventInfo(e.Title))
            .ToListAsync();

        List<EventsOverview.EventInfo> cancelledEvents = await context.Events
            .Where(e => e.Status.ToLower() == "cancelled")
            .Select(e => new EventsOverview.EventInfo(e.Title))
            .ToListAsync();

        
        return new EventsOverview.Answer(readyEvents, draftEvents, cancelledEvents);
        
    }
}