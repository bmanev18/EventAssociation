using System.Text.Json;
using EventAssociation.Infrastructure.EfcQueries.Models;

namespace EventAssociation.Infrastructure.EfcQueries.SeedFactories;

public class EventSeedFactory
{
    private static string EventsAsJson => File.ReadAllText(Path.Combine("Tests", "Mocks", "Events.json"));

    public static List<Event> CreateEvents()
    {
        var eventsTmps = JsonSerializer.Deserialize<List<TmpEvent>>(EventsAsJson)!;

        var events = eventsTmps.Select(e => new Event
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Status = e.Status,
            Type = e.Visibility, 
            StartDate = e.Start,
            EndDate = e.End,
            MaxParticipants = e.MaxGuests,
            LocationId = e.LocationId
        }).ToList();

        return events;
    }

    private record TmpEvent(
        string Id, 
        string Title, 
        string Description, 
        string Status, 
        string Visibility, 
        string Start, 
        string End, 
        int MaxGuests, 
        string LocationId
    );
}