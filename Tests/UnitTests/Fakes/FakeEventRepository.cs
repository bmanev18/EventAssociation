using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeEventRepository: IEventRepository
{
    List<Event> _events;

    public FakeEventRepository()
    {
           _events = new List<Event>();

            // First Event
            var location1 = Location.CreateLocation(
                LocationType.Outside, 
                LocationName.Create("Conference Hall").Unwrap(), 
                LocationCapacity.Create(500).Unwrap()
            ).Unwrap();

            var eventStartDate1 = new EventTime(DateTime.UtcNow.AddDays(1));
            var eventEndDate1 = new EventTime(DateTime.UtcNow.AddDays(2));

            var eventResult1 = Event.CreateEvent(location1, EventType.Public, eventStartDate1, eventEndDate1);
            var event1 = eventResult1.Unwrap();

            event1.ChangeTitle(EventTitle.CreateEventTitle("Tech Conference").Unwrap());
            event1.ChangeDescription(EventDescription.CreateEventDescription("A tech-focused event").Unwrap());

            _events.Add(event1);

            // Second Event
            var location2 = Location.CreateLocation(
                LocationType.Outside, 
                LocationName.Create("Meadows").Unwrap(), 
                LocationCapacity.Create(2000).Unwrap()
            ).Unwrap();

            var eventStartDate2 = new EventTime(DateTime.UtcNow.AddDays(3));
            var eventEndDate2 = new EventTime(DateTime.UtcNow.AddDays(4));

            var eventResult2 = Event.CreateEvent(location2, EventType.Public, eventStartDate2, eventEndDate2);
            var event2 = eventResult2.Unwrap();

            event2.ChangeTitle(EventTitle.CreateEventTitle("Music Festival").Unwrap());
            event2.ChangeDescription(EventDescription.CreateEventDescription("An outdoor music festival").Unwrap());

            _events.Add(event2);

            // Third Event
            var location3 = Location.CreateLocation(
                LocationType.Outside, 
                LocationName.Create("Co-working Space").Unwrap(), 
                LocationCapacity.Create(100).Unwrap()
            ).Unwrap();

            var eventStartDate3 = new EventTime(DateTime.UtcNow.AddDays(5));
            var eventEndDate3 = new EventTime(DateTime.UtcNow.AddDays(6));

            var eventResult3 = Event.CreateEvent(location3, EventType.Private, eventStartDate3, eventEndDate3);
            var event3 = eventResult3.Unwrap();

            event3.ChangeTitle(EventTitle.CreateEventTitle("Startup Meetup").Unwrap());
            event3.ChangeDescription(EventDescription.CreateEventDescription("A networking event for startups").Unwrap());

            _events.Add(event3);
    }
    
    public Task<Result<None>> CreateAsync(Event event_)
    {
        _events.Add(event_);
        return Task.FromResult(Result<None>.Ok(None.Value));
    }

    public Task<Result<Event>> GetAsync(EventId eventId)
    {
        var event_ = _events.FirstOrDefault(e => e.Id.Equals(eventId));
        return event_ != null 
            ? Task.FromResult(Result<Event>.Ok(event_)) 
            : Task.FromResult(Result<Event>.Err(new Error("", "Event not found")));
    }

    public Task<Result<None>> RemoveAsync(EventId eventId)
    {
        var event_ = _events.FirstOrDefault(e => e.Id.Equals(eventId));
        if (event_ != null)
        {
            _events.Remove(event_);
            return Task.FromResult(Result<None>.Ok(None.Value));
        }
        return Task.FromResult(Result<None>.Err(new Error("", "Event not found")));
    }

    public Task<Result<List<Event>>> GetAllEvents()
    {
        return Task.FromResult(Result<List<Event>>.Ok(_events));
    }
}