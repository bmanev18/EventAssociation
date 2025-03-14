using EventAssociation.Core.Domain.Aggregates.Events.Values;

namespace UnitTests.Features.Event.MakePublic;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Events.Values;

public class MakeEventPublicTest
{
    [Fact]
    public void ChangeEventTypeToPublic_ShouldReturnError_WhenEventIsCancelled()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var cancelledEvent = Event.CreateEvent(eventId).Unwrap();
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
        var eventId = new EventId(Guid.NewGuid());
        var activeEvent = Event.CreateEvent(eventId).Unwrap();
        activeEvent.ChangeEventStatusToReady();
        
        // Act
        var result = activeEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventType.Public, activeEvent.Type);
    }
    
    [Fact]
    public void ChangeEventTypeToPublic_ShouldUpdateType_WhenEventIsSetToDraft()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var draftEvent = Event.CreateEvent(eventId).Unwrap();
        
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
        var eventId = new EventId(Guid.NewGuid());
        var draftEvent = Event.CreateEvent(eventId).Unwrap();
        
        // Act
        var result = draftEvent.ChangeEventTypeToPublic();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventType.Public, draftEvent.Type);
    }
}