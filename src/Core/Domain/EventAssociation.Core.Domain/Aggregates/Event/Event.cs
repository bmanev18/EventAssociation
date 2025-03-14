using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event;

public class Event: AggregateRoot
{
internal EventId Id { get; }
    internal EventTitle Title { get; private set; }
    internal EventDescription Description { get; }
    internal EventTime StartDate { get; }
    internal EventTime EndDate { get; }
    internal EventMaxParticipants MaxParticipants { get; }
    internal EventType Type { get; }
    internal EventStatus Status { get; private set; }


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
        var eventTitle = EventTitle.CreateEventTitle("Working Title").Unwrap();
        var eventDescription = new EventDescription("");
        var maxParticipants = new EventMaxParticipants(5);
        var eventType = EventType.Private;
        var eventStatus = EventStatus.Draft;
        var event_ = new Event(id, eventTitle, eventDescription, null, null, maxParticipants, eventType, eventStatus);
        return Result<Event>.Ok(event_);
    }
    
    
    public Result<None> ChangeTitle(EventTitle title)
    {
        switch (this.Status)
        {
            case EventStatus.Active:
                return Result<None>.Err(new Error("100","Cannot update the title of an already active event"));
            
            case EventStatus.Cancelled:
                return Result<None>.Err(new Error("100","Cannot update the title of a cancelled event"));
            
            case EventStatus.Ready:
                this.Title = title;
                this.Status = EventStatus.Draft;   
                return Result<None>.Ok(None.Value);
            
            case EventStatus.Draft:
                this.Title = title;
                return Result<None>.Ok(None.Value);
            default:
                return Result<None>.Err(new Error("100","Invalid Event Status"));
        }
    }

    public Result<None> ChangeEventStatusToActive()
    {
        this.Status = EventStatus.Active;
        return Result<None>.Ok(None.Value);
    }
    
    public Result<None> ChangeEventStatusToCancelled()
    {
        this.Status = EventStatus.Cancelled;
        return Result<None>.Ok(None.Value);
    }
    
    public Result<None> ChangeEventStatusToDraft()
    {
        this.Status = EventStatus.Draft;
        return Result<None>.Ok(None.Value);
    }
    
    public Result<None> ChangeEventStatusToReady()
    {
        this.Status = EventStatus.Ready;
        return Result<None>.Ok(None.Value);
    }
}