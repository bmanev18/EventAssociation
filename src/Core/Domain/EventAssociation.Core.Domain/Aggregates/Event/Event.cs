using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event;

public class Event
{
    private EventId id;
    private EventTitle title;
    private EventDescription description;
    private EventTime startDate;
    private EventTime endDate;
    private EventMaxParticipants maxParticipants;
    private EventType type;
    private EventStatus status;
    
    // private GuestList guestList;

    private Event(EventId id, EventTitle title, EventDescription description, EventTime startDate, EventTime endDate, EventMaxParticipants maxParticipants, EventType type, EventStatus status)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.startDate = startDate;
        this.endDate = endDate;
        this.maxParticipants = maxParticipants;
        this.type = type;
        this.status = status;
    }

    public Result<Event> CreateEvent(EventId id)
    {
        // var eventTitle = new EventTitle("Working Title");
        // var eventDescription = new EventDescription(" ");
        // Event event = new Event(id, title, eventDescription, startDate, endDate, maxParticipants, type, status); 
        // return new Result<Event>(event);

    }
    
    
    
    
}