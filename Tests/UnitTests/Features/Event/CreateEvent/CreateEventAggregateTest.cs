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
        Assert.Equal(EventTitle.CreateEventTitle("Working Title").Unwrap(),createdEvent.Title);
        Assert.Equal(new EventDescription(""), createdEvent.Description);
        Assert.Equal(new EventMaxParticipants(5), createdEvent.MaxParticipants);
        Assert.Equal(EventType.Private, createdEvent.Type);
        Assert.Equal(EventStatus.Draft, createdEvent.Status);
    }
    

     
    
    }