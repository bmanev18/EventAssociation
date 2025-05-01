using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.QueryContracts.Contract;
using EventAssociation.Core.QueryContracts.Queries;
using EventAssociation.Infrastructure.EfcQueries.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.EfcQueries.Queries;

public class AvailableLocationsHandler(EventAssociationProductionContext context)
    : IQueryHandler<AvailableLocations.Query, AvailableLocations.Answer>
{
    public async Task<AvailableLocations.Answer> HandleAsync(AvailableLocations.Query query)
    {
        var events = await context.Events
            .Include(e => e.Location)
            .ToListAsync();

        var bookedLocations = events
            .Where(e => isBooked(e.StartDate, e.EndDate, query.startDate))
            .Select(e => e.Location)
            .Distinct()
            .ToList();

        var locations = await context.Locations.ToListAsync();

        var availableLocations = locations
            .Where(location => bookedLocations.All(b => b.Id != location.Id))
            .Select(l => new AvailableLocations.LocationsInfo(l.LocationName, "", l.LocationCapacity, l.Status))
            .ToList();

        

        return new AvailableLocations.Answer(availableLocations);
    }

    
    private bool IsBetween(EventTime startDate, EventTime endDate, EventTime requestDate)
    {
        var start = startDate.Value;
        var end = endDate.Value;
        var request = requestDate.Value;

        if (start <= request && request <= end)
        {
            return true;
        }

        return false;
    }
    
    private bool AreDateEquals(EventTime date1, EventTime date2)
    {
        return date1.Value == date2.Value;

    }
    
    private bool isBooked(string startDate, string endDate, string requestDate)
    {
        var startDateET = EventTime.Create(startDate).Unwrap();
        var endDateET = EventTime.Create(endDate).Unwrap();
        var requestDateET = EventTime.Create(requestDate).Unwrap();
        
        
        if (!IsBetween(startDateET, endDateET, requestDateET) && !AreDateEquals(startDateET, requestDateET))
        {
            return false;
        }
        return true;
    }
}