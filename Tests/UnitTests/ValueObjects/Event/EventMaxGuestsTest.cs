using EventAssociation.Core.Domain.Aggregates.Event.Values;

namespace UnitTests.ValueObjects.Event;

public class EventMaxGuestsTest
{
    [Fact]
    public void Create_ShouldFail_WhenParticipantsAreLessThanFive()
    {
        // Act
        var result = EventMaxParticipants.Create(4);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The max participants cannot be less than 5", result.UnwrapErr().First().Message);
    }

    [Fact]
    public void Create_ShouldFail_WhenParticipantsExceedFifty()
    {
        // Act
        var result = EventMaxParticipants.Create(51);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The max participants cannot be greater than 50", result.UnwrapErr().First().Message);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenParticipantsAreWithinValidLimits()
    {
        // Act
        var result = EventMaxParticipants.Create(25);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Unwrap().Value);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenParticipantsAreAtLowerBoundary()
    {
        // Act
        var result = EventMaxParticipants.Create(5);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Unwrap().Value);
    }

    [Fact]
    public void Create_ShouldSucceed_WhenParticipantsAreAtUpperBoundary()
    {
        // Act
        var result = EventMaxParticipants.Create(50);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(50, result.Unwrap().Value);
    }
}