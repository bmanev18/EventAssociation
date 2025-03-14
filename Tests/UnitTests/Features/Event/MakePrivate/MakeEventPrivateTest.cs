namespace UnitTests.Features.Event.MakePrivate;

using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using Xunit;
using EventAssociation.Core.Tools.OperationResult;


public class MakeEventPrivateTest
{
    [Fact]
        public void ChangeEventType_ShouldReturnError_WhenEventIsActiveOrCancelled()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var activeEvent = Event.CreateEvent(eventId).Unwrap();
            activeEvent.ChangeEventStatusToActive();
            
            var cancelledEvent = Event.CreateEvent(eventId).Unwrap();
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
            var eventId = new EventId(Guid.NewGuid());
            var eventWithPublicType = Event.CreateEvent(eventId).Unwrap();
            eventWithPublicType.ChangeEventStatusToReady();
            eventWithPublicType.ChangeEventTypeToPublic();

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
            var eventId = new EventId(Guid.NewGuid());
            var eventWithPrivateType = Event.CreateEvent(eventId).Unwrap();
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
            var eventId = new EventId(Guid.NewGuid());
            var eventWithInvalidType = Event.CreateEvent(eventId).Unwrap();
            eventWithInvalidType.ChangeEventStatusToCancelled();

            // Act
            var result = eventWithInvalidType.ChangeEventTypeToPrivate();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot change the event type of an active or cancelled event", result.UnwrapErr().First().Message);
        }
    }