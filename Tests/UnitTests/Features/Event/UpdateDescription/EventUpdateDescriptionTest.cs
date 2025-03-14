﻿
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.UpdateDescription;
using Xunit;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;
public class EventUpdateDescriptionTest
{
    
    [Fact]
    public void ChangeDescription_ShouldReturnError_WhenEventIsActive()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            
        var activeEvent = Event.CreateEvent(location, EventType.Private).Unwrap();
        activeEvent.ChangeEventStatusToActive();
        var newDescription = EventDescription.CreateEventDescription("New Event Description");
        
        // Act
        var result = activeEvent.ChangeDescription(newDescription.Unwrap());
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Cannot update the description of an already active event", result.UnwrapErr().First().Message);
    }
    
    [Fact]
        public void ChangeDescription_ShouldReturnError_WhenEventIsCancelled()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var cancelledEvent = Event.CreateEvent(location, EventType.Private).Unwrap();
            cancelledEvent.ChangeEventStatusToCancelled();
            var newDescription = EventDescription.CreateEventDescription("New Event Description");
        
            // Act
            var result = cancelledEvent.ChangeDescription(newDescription.Unwrap());
        
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot update the description of a cancelled event", result.UnwrapErr().First().Message);
        }
        
        [Fact]
        public void ChangeDescription_ShouldUpdateDescription_WhenEventIsReady()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var readyEvent = Event.CreateEvent(location, EventType.Private).Unwrap();
            readyEvent.ChangeEventStatusToReady();
            var newDescription = EventDescription.CreateEventDescription("New Event Title");
        
            // Act
            var result = readyEvent.ChangeDescription(newDescription.Unwrap());
        
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription.Unwrap(), readyEvent.Description);
            Assert.Equal(EventStatus.Draft, readyEvent.Status);
        }
        
        [Fact]
        public void ChangeDescription_ShouldUpdateDescription_WhenEventIsDraft()
        {
            // Arrange
            
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var draftEvent = Event.CreateEvent(location, EventType.Private).Unwrap();
            draftEvent.ChangeEventStatusToDraft();
            var newDescription = EventDescription.CreateEventDescription("New Event Title");
        
            // Act
            var result = draftEvent.ChangeDescription(newDescription.Unwrap());
        
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription.Unwrap(), draftEvent.Description);
            Assert.Equal(EventStatus.Draft, draftEvent.Status);
        }
    
}