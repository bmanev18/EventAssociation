using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event;

public class Event : AggregateRoot
{
    internal EventId Id { get; }
    internal EventTitle Title { get; private set; }
    internal EventDescription Description { get; private set; }
    internal EventTime? StartDate { get; }
    internal EventTime? EndDate { get; }
    internal EventMaxParticipants MaxParticipants { get; private set; }
    internal EventType Type { get; private set; }
    internal EventStatus Status { get; private set; }

    internal Location Location { get; private set; }


    // private GuestList guestList;

    private Event(EventId id, EventTitle title, EventDescription description, EventTime? startDate, EventTime? endDate,
        EventMaxParticipants maxParticipants, EventType type, EventStatus status, Location location)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.MaxParticipants = maxParticipants;
        this.Type = type;
        this.Status = status;
        this.Location = location;
    }

    public static Result<Event> CreateEvent(Location location, EventType eventType)
    {
        var id = new EventId(Guid.NewGuid());
        var eventTitle = EventTitle.CreateEventTitle("Working Title").Unwrap();
        var eventDescription = EventDescription.CreateEventDescription("").Unwrap();
        var maxParticipants = EventMaxParticipants.Create(5).Unwrap();
        var eventStatus = EventStatus.Draft;
        var newEvent = new Event(id, eventTitle, eventDescription, null, null, maxParticipants, eventType, eventStatus,
            location);
        return Result<Event>.Ok(newEvent);
    }


    public Result<None> ChangeTitle(EventTitle title)
    {
        switch (this.Status)
        {
            case EventStatus.Active:
                return Result<None>.Err(new Error("100", "Cannot update the title of an already active event"));

            case EventStatus.Cancelled:
                return Result<None>.Err(new Error("100", "Cannot update the title of a cancelled event"));

            case EventStatus.Ready:
                this.Title = title;
                this.Status = EventStatus.Draft;
                return Result<None>.Ok(None.Value);

            case EventStatus.Draft:
                this.Title = title;
                return Result<None>.Ok(None.Value);
            default:
                return Result<None>.Err(new Error("100", "Invalid Event Status"));
        }
    }

    public Result<None> ChangeDescription(EventDescription description)
    {
        switch (this.Status)
        {
            case EventStatus.Active:
                return Result<None>.Err(new Error("100", "Cannot update the description of an already active event"));

            case EventStatus.Cancelled:
                return Result<None>.Err(new Error("100", "Cannot update the description of a cancelled event"));

            case EventStatus.Ready:
                this.Description = description;
                this.Status = EventStatus.Draft;
                return Result<None>.Ok(None.Value);

            case EventStatus.Draft:
                this.Description = description;
                return Result<None>.Ok(None.Value);

            default:
                return Result<None>.Err(new Error("100", "Invalid Event Status"));
        }
    }

    public Result<None> UpdateMaxNumberOfParticipants(int maxParticipants)
    {
        if (Status == EventStatus.Active)
        {
            if (maxParticipants < MaxParticipants.Value)
            {
                return Result<None>.Err(new Error("100",
                    "Cannot reduce the max participants for an active event, it can only be increased"));
            }
        }

        if (Status == EventStatus.Cancelled)
        {
            return Result<None>.Err(new Error("100",
                "Cannot modify the max participants for a cancelled event"));
        }

        if (Location.LocationCapacity.Value < maxParticipants)
        {
            return Result<None>.Err(new Error(" ",
                "Cannot have more participants than the maximum capacity allowed by the location."));
        }

        var updatedMaxParticipants = EventMaxParticipants.Create(maxParticipants).Unwrap();
        this.MaxParticipants = updatedMaxParticipants;
        return Result<None>.Ok(None.Value);
    }

    public Result<None> ChangeEventStatusToActive()
    {
        // ChangeEventStatusToReady will check for invalid fields and also make sure the status is Ready
        // F1 + F2
        var eventIsReady = ChangeEventStatusToReady();
        if (!eventIsReady.IsSuccess)
        {
            return eventIsReady;
        }
        
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
        // F1 + F2
        if (Status != EventStatus.Draft)
        {
            return Result<None>.Err(new Error("100", "Cannot change the status of an event, which is not in draft"));
        }

        var results = new List<Result<None>>();
        // F4
        if (Title.IsEmptyOrDefualt())
        {
            results.Add(Result<None>.Err(new Error("100", "Title cannot be empty or kept default")));
        }
        // F1 time checks
        else if (StartDate == null || EndDate == null)
        {
            results.Add(Result<None>.Err(new Error("100", "Start and end dates are required")));
        }
        
        // F3
        var isEventInTheFuture = StartDate.LaterThanNow();
        if (!isEventInTheFuture.IsSuccess)
        {
            return isEventInTheFuture;
        }

        // TODO add additional checks:

        var validate = Result<None>.AssertResponses(results);
        if (!validate.IsSuccess)
        {
            return validate;
        }

        this.Status = EventStatus.Ready;
        return Result<None>.Ok(None.Value);
    }

    public Result<None> ChangeEventTypeToPublic()
    {
        if (this.Status == EventStatus.Cancelled)
        {
            return Result<None>.Err(new Error("100", "Cannot change the event type of a cancelled event"));
        }

        this.Type = EventType.Public;
        return Result<None>.Ok(None.Value);
    }

    public Result<None> ChangeEventTypeToPrivate()
    {
        if (this.Status == EventStatus.Active || this.Status == EventStatus.Cancelled)
        {
            return Result<None>.Err(new Error("100", "Cannot change the event type of an active or cancelled event"));
        }

        if (this.Type == EventType.Public)
        {
            this.Type = EventType.Private;
            ChangeEventStatusToDraft();
            return Result<None>.Ok(None.Value);
        }

        return Result<None>.Ok(None.Value);
    }

    public Result<None> ChangeTimes(EventTime startTime, EventTime endTime)
    {
        if (Status == EventStatus.Active || Status == EventStatus.Cancelled)
        {
            return Result<None>.Err(new Error("Code", "Active or Canceled events cannot be edited"));
        }

        var valid = ValidateTimes(startTime, endTime);
        if (!valid.IsSuccess)
        {
            return valid;
        }

        if (Status == EventStatus.Ready)
        {
            ChangeEventStatusToDraft();
        }

        return Result<None>.Ok(None.Value);
    }

    private Result<None> ValidateTimes(EventTime startTime, EventTime endTime)
    {
        if (!startTime.IsBefore(endTime).IsSuccess)
        {
            return Result<None>.Err(new Error("100", "Start time is not before end time"));
        }

        var results = new List<Result<None>>
        {
            endTime.AtLeastOneHourSince(startTime),
            endTime.IntervalLessThan10Hours(startTime),
            startTime.After8()
        };

        var preprocess = Result<None>.AssertResponses(results);
        if (!preprocess.IsSuccess)
        {
            return preprocess;
        }

        if (startTime.isTheSameDayAs(endTime).IsSuccess)
        {
            return endTime.Before12AM();
        }
        else if (endTime.IsNextDayFrom(startTime).IsSuccess)
        {
            return endTime.Before1Am();
        }
        else
        {
            return Result<None>.Err(new Error("100", "End time is more than one day after Start time"));
        }
    }
}