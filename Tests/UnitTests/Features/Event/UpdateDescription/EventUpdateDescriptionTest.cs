
using EventAssociation.Core.Domain.Aggregates.Event.Values;
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
            
        
        newEvent.ChangeEventStatusToActive();
        var newDescription = EventDescription.CreateEventDescription("New Event Description");
        
        // Act
        var result = newEvent.ChangeDescription(newDescription.Unwrap());
        
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
            var cancelledEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
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
            var readyEvent = Event.CreateEvent(location, EventType.Private, null ,null).Unwrap();
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
            var draftEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
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