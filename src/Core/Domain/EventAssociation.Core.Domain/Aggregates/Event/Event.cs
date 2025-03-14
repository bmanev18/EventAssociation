using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event;

public class Event: AggregateRoot
{
internal EventId Id { get; }
    internal EventTitle Title { get; }
    internal EventDescription Description { get; }
    internal EventTime StartDate { get; }
    internal EventTime EndDate { get; }
    internal EventMaxParticipants MaxParticipants { get; }
    internal EventType Type { get; }
    internal EventStatus Status { get; }


    // private GuestList guestList;

    private Event(EventId id, EventTitle title, EventDescription description, EventTime startDate, EventTime endDate,
        EventMaxParticipants maxParticipants, EventType type, EventStatus status)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.MaxParticipants = maxParticipants;
        this.Type = type;
        this.Status = status;
    }

    public static Result<Event> CreateEvent(EventId id)
    {
        var eventTitle = new EventTitle("Working Title");
        var eventDescription = new EventDescription("");
        var maxParticipants = new EventMaxParticipants(5);
        var eventType = EventType.Private;
        var eventStatus = EventStatus.Draft;
        var event_ = new Event(id, eventTitle, eventDescription, null, null, maxParticipants, eventType, eventStatus);
        return Result<Event>.Ok(event_);
    }
}