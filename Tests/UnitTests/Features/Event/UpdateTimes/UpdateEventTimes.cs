using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.UpdateTimes;
using EventAssociation.Core.Domain.Aggregates.Event;

public class UpdateEventTimes
{
    private Event DummyEvent()
    {
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var title = EventTitle.CreateEventTitle("Birthday Party").Unwrap();
        var description = EventDescription.CreateEventDescription("Surprised event").Unwrap();
            
        var newEvent = EventAssociation.Core.Domain.Aggregates.Event.Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        var setTitle = newEvent.ChangeTitle(title);
        Assert.True(setTitle.IsSuccess);

        var setDescription = newEvent.ChangeDescription(description);
        Assert.True(setDescription.IsSuccess);
            
        var startTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(13, 00));
        var endTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00));
        var setTimes = newEvent.ChangeTimes(new EventTime(startTime), new EventTime(endTime));
        Assert.True(setTimes.IsSuccess);
        
        return newEvent;
    }
    
    
}