using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.MakePublic;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Events.Values;

public class MakeEventPublicTest
{
    private Event DummyPrivateEvent()
    {
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var title = EventTitle.CreateEventTitle("Birthday Party").Unwrap();
        var description = EventDescription.CreateEventDescription("Surprised event").Unwrap();
            
        var newEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
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
    [Fact]
    public void ChangeEventTypeToPublic_ShouldReturnError_WhenEventIsCancelled()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            
        var cancelledEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        cancelledEvent.ChangeEventStatusToCancelled();

        // Act
        var result = cancelledEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Cannot change the event type of a cancelled event", result.UnwrapErr().First().Message);
    }

    [Fact]
    public void ChangeEventTypeToPublic_ShouldUpdateType_WhenEventIsNotCancelled()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            
        var readyEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        readyEvent.ChangeEventStatusToReady();
        
        // Act
        var result = readyEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventType.Public, readyEvent.Type);
    }
    
    [Fact]
    public void ChangeEventTypeToPublic_ShouldUpdateType_WhenEventIsSetToDraft()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            
        var draftEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        
        // Act
        var result = draftEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventType.Public, draftEvent.Type);
    }
    
    [Fact]
    public void ChangeEventTypeToPublic_ShouldUpdateType_WhenEventIsSetToActive()
    {
        // Arrange
        var activeEvent = DummyPrivateEvent();
        activeEvent.ChangeEventStatusToActive();
        Assert.Equal(EventStatus.Active, activeEvent.Status);
        
        // Act
        var result = activeEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventType.Public, activeEvent.Type);
    }
}