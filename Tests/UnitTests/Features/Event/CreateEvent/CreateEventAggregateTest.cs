namespace UnitTests.Features.Event.CreateEvent;
using Xunit;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Tools.OperationResult;

public class CreateEventAggregateTest
{
    [Fact]
    public void CreateEvent_ShouldReturnSuccessResult()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());

        // Act
        var result = Event.CreateEvent(eventId);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Unwrap());
    }

    [Fact]
    public void CreateEvent_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());

        // Act
        var result = Event.CreateEvent(eventId);
        var createdEvent = result.Unwrap();

        // Assert
        Assert.Equal(eventId, createdEvent.Id);
        Assert.Equal(new EventTitle("Working Title"), createdEvent.Title);
        Assert.Equal(new EventDescription(""), createdEvent.Description);
        Assert.Equal(new EventMaxParticipants(5), createdEvent.MaxParticipants);
        Assert.Equal(EventType.Private, createdEvent.Type);
        Assert.Equal(EventStatus.Draft, createdEvent.Status);
    }
    
    [Fact]
    public void ChangeTitle_ShouldReturnError_WhenEventIsActive()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var activeEvent = Event.CreateEvent(eventId).Unwrap();
        activeEvent.ChangeEventStatusToActive();
        var newTitle = new EventTitle("New Event Title");

        // Act
        var result = activeEvent.ChangeTitle(newTitle);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Cannot update the title of an already active event", result.UnwrapErr().First().Message);
    }
     [Fact]
        public void ChangeTitle_ShouldReturnError_WhenEventIsCancelled()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var cancelledEvent = Event.CreateEvent(eventId).Unwrap();
            cancelledEvent.ChangeEventStatusToCancelled();
            var newTitle = new EventTitle("New Event Title");

            // Act
            var result = cancelledEvent.ChangeTitle(newTitle);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot update the title of a cancelled event", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void ChangeTitle_ShouldUpdateTitle_WhenEventIsReady()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var readyEvent = Event.CreateEvent(eventId).Unwrap();
            readyEvent.ChangeEventStatusToReady();
            var newTitle = new EventTitle("New Event Title");

            // Act
            var result = readyEvent.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, readyEvent.Title);
            Assert.Equal(EventStatus.Draft, readyEvent.Status);
        }

        [Fact]
        public void ChangeTitle_ShouldUpdateTitle_WhenEventIsDraft()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var draftEvent = Event.CreateEvent(eventId).Unwrap();
            draftEvent.ChangeEventStatusToDraft();
            var newTitle = new EventTitle("New Event Title");

            // Act
            var result = draftEvent.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, draftEvent.Title);
            Assert.Equal(EventStatus.Draft, draftEvent.Status);
        }
    
    }