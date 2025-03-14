namespace UnitTests.Features.Event.SetMaxGuests;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Domain.Aggregates.Event;



public class SetMaxGuestsTest
{
      [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenExceedingLocationCapacity()
    {
        // Arrange
        var locationCapacity = LocationCapacity.Create(50).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private).Unwrap();
        
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(60);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("You cannot have more participants than the maximum capacity allowed by the location.", result.UnwrapErr().First().Message);
    }

    [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenReducingParticipantsForActiveEvent()
    {
        // Arrange
        var locationCapacity = LocationCapacity.Create(100).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private).Unwrap();
        
        event_.UpdateMaxNumberOfParticipants(80);
        event_.ChangeEventStatusToActive();
    
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(70);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Cannot reduce the max participants for an active event, it can only be increased.", result.UnwrapErr().First().Message);
    }
    
    [Fact]
    public void UpdateMaxNumberOfParticipants_ShouldFail_WhenEventIsCancelled()
    {
        // Arrange
        
        var locationCapacity = LocationCapacity.Create(100).Unwrap();
        var locationName = LocationName.Create("Meadows").Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var event_ = Event.CreateEvent(location, EventType.Private).Unwrap();
        event_.ChangeEventStatusToCancelled();
        
        
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(90);
    
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
        var event_ = Event.CreateEvent(location, EventType.Private).Unwrap();
    
        // Act
        var result = event_.UpdateMaxNumberOfParticipants(80);
    
        // Assert
        Assert.True(result.IsSuccess);
    }
}