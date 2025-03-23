using EventAssociation.Core.Domain.Aggregates.Event.Values;

namespace UnitTests.Features.Event.SetMaxGuests;

using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Domain.Aggregates.Event;



public class SetMaxGuestsTest
{
      [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenExceedingLocationCapacity()
    {
        // Arrange
        var locationCapacity = LocationCapacity.Create(40).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(EventMaxParticipants.Create(49).Unwrap());
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cannot have more participants than the maximum capacity allowed by the location.", result.UnwrapErr().First().Message);
    }

    [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenReducingParticipantsForActiveEvent()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(100).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var title = EventTitle.CreateEventTitle("Birthday Party").Unwrap();
        var description = EventDescription.CreateEventDescription("Surprised event").Unwrap();

        var newEvent = Event.CreateEvent(location, EventType.Public, null, null).Unwrap();
        var setTitle = newEvent.ChangeTitle(title);
        Assert.True(setTitle.IsSuccess);

        var setDescription = newEvent.ChangeDescription(description);
        Assert.True(setDescription.IsSuccess);

        var startTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(13, 00));
        var endTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00));
        var setTimes = newEvent.ChangeTimes(new EventTime(startTime), new EventTime(endTime));
        Assert.True(setTimes.IsSuccess);
        
        
        newEvent.UpdateMaxNumberOfParticipants(EventMaxParticipants.Create(30).Unwrap());
        newEvent.ChangeEventStatusToActive();
        Assert.Equal(EventStatus.Active, newEvent.Status);
    
        // Act
        var result = newEvent.UpdateMaxNumberOfParticipants(EventMaxParticipants.Create(20).Unwrap());
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cannot reduce the max participants for an active event, it can only be increased", result.UnwrapErr().First().Message);
    }
    
    [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenEventIsCancelled()
    {
        // Arrange
        
        var locationCapacity = LocationCapacity.Create(100).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
        event_.ChangeEventStatusToCancelled();
        
        var newMaxNumberOfParticipants = EventMaxParticipants.Create(40).Unwrap();
        
        
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(newMaxNumberOfParticipants);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cannot modify the max participants for a cancelled event", result.UnwrapErr().First().Message);
    }
    
    [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldSucceed_WhenUpdatingWithinAllowedConstraints()
    {
        // Arrange
        var locationCapacity = LocationCapacity.Create(100).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
    
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(EventMaxParticipants.Create(40).Unwrap());
    
        // Assert
        Assert.True(result.IsSuccess);
    }
}