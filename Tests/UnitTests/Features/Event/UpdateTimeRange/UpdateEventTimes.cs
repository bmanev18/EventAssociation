using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

// namespace UnitTests.Features.Event.UpdateTimeRange;

namespace EventAssociation.Tests;

public class UpdateEventTimes
{
    private static Event DummyEvent()
    {
        var id = new EventId(Guid.NewGuid());
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();
        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
        var title = EventTitle.CreateEventTitle("Birthday Party").Unwrap();
        var description = EventDescription.CreateEventDescription("Surprised event").Unwrap();

        var maxParticipants = EventMaxParticipants.Create(5).Unwrap();

        var startTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(13, 00));
        var endTime = new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00));

        var newEvent = new Event(id, title, description, new EventTime(startTime), new EventTime(endTime),
            maxParticipants, EventType.Private, EventStatus.Draft, location);

        return newEvent;
    }

    [Fact]
    public void ChangeTimes_ActiveEvent_ReturnsError()
    {
        // Arrange
        var newEvent = DummyEvent();
        newEvent.ChangeEventStatusToActive();
        var startTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00)));
        var endTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(17, 00)));


        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ChangeTimes_CancelledEvent_ReturnsError()
    {
        // Arrange
        var newEvent = DummyEvent();
        newEvent.ChangeEventStatusToCancelled();
        var startTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00)));
        var endTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(17, 00)));



        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ChangeTimes_StartTimeAfterEndTime_ReturnsError()
    {
        // Arrange
        var newEvent = DummyEvent();
        newEvent.ChangeEventStatusToActive();
        var startTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(19, 00)));
        var endTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(17, 00)));



        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData("2024-01-01T07:00:00", "2024-01-01T10:00:00")]
    [InlineData("2024-01-01T10:00:00", "2024-01-01T10:30:00")]
    [InlineData("2024-01-01T10:00:00", "2024-01-01T21:00:01")]
    [InlineData("2024-01-01T10:00:00", "2024-01-01T23:59:59")]
    [InlineData("2024-01-01T23:00:00", "2024-01-02T00:59:59")]
    [InlineData("2024-01-01T10:00:00", "2024-01-03T10:00:00")]
    public void ChangeTimes_InvalidTimes_ReturnsError(string startTimeStr, string endTimeStr)
    {
        // Arrange
        var newEvent = DummyEvent();
        newEvent.ChangeEventStatusToActive();
        var startTime = new EventTime(DateTime.Parse(startTimeStr));
        var endTime = new EventTime(DateTime.Parse(endTimeStr));

        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void ChangeTimes_ValidTimes_ChangesTimesAndReturnsOk()
    {
        // Arrange
        var newEvent = DummyEvent();
        var startTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00)));
        var endTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(17, 00)));


        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, newEvent.StartDate);
        Assert.Equal(endTime, newEvent.EndDate);
    }

    [Fact]
    public void ChangeTimes_ReadyEvent_ChangesStatusToDraft()
    {
        // Arrange
        var newEvent = DummyEvent();
        newEvent.ChangeEventStatusToReady();
        var startTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(15, 00)));
        var endTime = new EventTime(new DateTime(new DateOnly(2026, 12, 31), new TimeOnly(17, 00)));


        // Act
        var result = newEvent.ChangeTimes(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Draft, newEvent.Status);
    }
}