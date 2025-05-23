using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.MakePrivate;

using EventAssociation.Core.Domain.Aggregates.Event;
using Xunit;
using EventAssociation.Core.Tools.OperationResult;


public class MakeEventPrivateTest
{
    private Event DummyPublicEvent()
    {
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
        
        return newEvent;
    }
    
    [Fact]
        public void ChangeEventType_ShouldReturnError_WhenEventIsActiveOrCancelled()
        {
            // Arrange
            var activeEvent = DummyPublicEvent();
            activeEvent.ChangeEventStatusToActive();
            Assert.Equal(EventStatus.Active,activeEvent.Status);
            
            var cancelledEvent = DummyPublicEvent();
            cancelledEvent.ChangeEventStatusToCancelled();

            // Act
            var activeResult = activeEvent.ChangeEventTypeToPrivate();
            var cancelledResult = cancelledEvent.ChangeEventTypeToPrivate();

            // Assert for active event
            Assert.False(activeResult.IsSuccess);
            Assert.Contains("Cannot change the event type of an active or cancelled event", activeResult.UnwrapErr().First().Message);

            // Assert for cancelled event
            Assert.False(cancelledResult.IsSuccess);
            Assert.Contains("Cannot change the event type of an active or cancelled event", cancelledResult.UnwrapErr().First().Message);
        }

        [Fact]
        public void ChangeEventType_ShouldUpdateType_WhenEventIsPublic()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            
            var eventWithPublicType = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            eventWithPublicType.ChangeEventStatusToReady();

            // Act
            var result = eventWithPublicType.ChangeEventTypeToPrivate();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventType.Private, eventWithPublicType.Type);
        }

        // Test for the case where the event type is not Public (i.e., already Private or other types)
        [Fact]
        public void ChangeEventType_ShouldNotChangeType_WhenEventIsNotPublic()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();

            var eventWithPrivateType = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            eventWithPrivateType.ChangeEventStatusToReady();

            // Act
            var result = eventWithPrivateType.ChangeEventTypeToPrivate();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(EventType.Private, eventWithPrivateType.Type); // No change since it's already Private
        }

        [Fact]
        public void ChangeEventType_ShouldReturnError_WhenEventStatusIsInvalidForChange()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();

            var eventWithPrivateType = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            eventWithPrivateType.ChangeEventStatusToReady();

            var eventWithInvalidType = Event.CreateEvent(location, EventType.Public, null, null).Unwrap();
            eventWithInvalidType.ChangeEventStatusToCancelled();
            

            // Act
            var result = eventWithInvalidType.ChangeEventTypeToPrivate();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot change the event type of an active or cancelled event", result.UnwrapErr().First().Message);
        }
    }