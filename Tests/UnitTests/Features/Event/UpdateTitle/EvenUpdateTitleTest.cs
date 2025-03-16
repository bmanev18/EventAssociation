using System.Diagnostics;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.CreateEvent
{
    using Xunit;
    using EventAssociation.Core.Domain.Aggregates.Events.Values;
    using EventAssociation.Core.Domain.Aggregates.Event;
    using EventAssociation.Core.Tools.OperationResult;

    public class EventTitleTests
    {
        [Fact]
        public void ChangeTitle_ShouldReturnError_WhenEventIsActive()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var title = EventTitle.CreateEventTitle("Birthday Party").Unwrap();
            var description = EventDescription.CreateEventDescription("Surprised event").Unwrap();
            
            var activeEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            var setTitle = activeEvent.ChangeTitle(title);
            Assert.True(setTitle.IsSuccess);

            var setDescription = activeEvent.ChangeDescription(description);
            Assert.True(setDescription.IsSuccess);
            
            var startTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(13, 00));
            var endTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00));
            var setTimes = activeEvent.ChangeTimes(new EventTime(startTime), new EventTime(endTime));
            Assert.True(setTimes.IsSuccess);
            
            activeEvent.ChangeEventStatusToActive();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");
            
            Assert.Equal(EventStatus.Active, activeEvent.Status);
            // Act
            var result = activeEvent.ChangeTitle(newTitle.Unwrap());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot update the title of an already active event", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void ChangeTitle_ShouldReturnError_WhenEventIsCancelled()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var cancelledEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            cancelledEvent.ChangeEventStatusToCancelled();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");

            // Act
            var result = cancelledEvent.ChangeTitle(newTitle.Unwrap());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot update the title of a cancelled event", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void ChangeTitle_ShouldUpdateTitle_WhenEventIsReady()
        {
            // Arrange
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var readyEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            readyEvent.ChangeEventStatusToReady();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");

            // Act
            var result = readyEvent.ChangeTitle(newTitle.Unwrap());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle.Unwrap(), readyEvent.Title);
            Assert.Equal(EventStatus.Draft, readyEvent.Status);
        }

        [Fact]
        public void ChangeTitle_ShouldUpdateTitle_WhenEventIsDraft()
        {
            // Arrange

            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(20).Unwrap();
            var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
            var draftEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();
            draftEvent.ChangeEventStatusToDraft();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");

            // Act
            var result = draftEvent.ChangeTitle(newTitle.Unwrap());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle.Unwrap(), draftEvent.Title);
            Assert.Equal(EventStatus.Draft, draftEvent.Status);
        }
    }
}