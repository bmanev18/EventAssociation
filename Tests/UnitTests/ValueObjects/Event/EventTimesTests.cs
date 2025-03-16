using EventAssociation.Core.Domain.Aggregates.Event.Values;

namespace UnitTests.ValueObjects.Event;

public class EventTimesTests
{
    [Theory]
    [InlineData("2025-03-17T10:00:00", "2025-03-17T11:00:00", true)]
    [InlineData("2025-03-17T12:00:00", "2025-03-17T11:00:00", false)]
    public void IsBefore_ShouldReturnExpectedResult(string first, string second, bool expected)
    {
        var time1 = new EventTime(DateTime.Parse(first));
        var time2 = new EventTime(DateTime.Parse(second));
        var result = time1.IsBefore(time2);
        Assert.Equal(expected, result.IsSuccess);
    }
    
    [Theory]
    [InlineData("2025-03-17T10:00:00", "2025-03-17T10:30:00", false)]
    [InlineData("2025-03-17T10:00:00", "2025-03-17T11:00:00", true)]
    public void AtLeastOneHourSince_ShouldReturnExpectedResult(string first, string second, bool expected)
    {
        var time1 = new EventTime(DateTime.Parse(first));
        var time2 = new EventTime(DateTime.Parse(second));
        var result = time2.AtLeastOneHourSince(time1);
        Assert.Equal(expected, result.IsSuccess);
    }
    
    [Theory]
    [InlineData("2025-03-17T08:00:00", true)]
    [InlineData("2025-03-17T07:59:00", false)]
    public void After8_ShouldReturnExpectedResult(string time, bool expected)
    {
        var eventTime = new EventTime(DateTime.Parse(time));
        var result = eventTime.After8();
        Assert.Equal(expected, result.IsSuccess);
    }

    [Theory]
    [InlineData("2025-03-17T22:59:00", true)]
    [InlineData("2025-03-17T23:59:00", false)]
    public void Before12Am_ShouldReturnExpectedResult(string time, bool expected)
    {
        var eventTime = new EventTime(DateTime.Parse(time));
        var result = eventTime.Before12Am();
        Assert.Equal(expected, result.IsSuccess);
    }

    [Theory]
    [InlineData("2025-03-17T00:30:00", true)]
    [InlineData("2025-03-17T01:00:00", false)]
    public void Before1Am_ShouldReturnExpectedResult(string time, bool expected)
    {
        var eventTime = new EventTime(DateTime.Parse(time));
        var result = eventTime.Before1Am();
        Assert.Equal(expected, result.IsSuccess);
    }

    [Theory]
    [InlineData("2025-03-17T10:00:00", "2025-03-18T10:00:00", true)]
    [InlineData("2025-03-17T10:00:00", "2025-03-19T10:00:00", false)]
    public void IsNextDayFrom_ShouldReturnExpectedResult(string first, string second, bool expected)
    {
        var time1 = new EventTime(DateTime.Parse(first));
        var time2 = new EventTime(DateTime.Parse(second));
        var result = time2.IsNextDayFrom(time1);
        Assert.Equal(expected, result.IsSuccess);
    }

    [Fact]
    public void LaterThanNow_ShouldReturnTrueForFutureDate()
    {
        var futureTime = new EventTime(DateTime.Now.AddHours(1));
        var result = futureTime.LaterThanNow();
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void LaterThanNow_ShouldReturnFalseForPastDate()
    {
        var pastTime = new EventTime(DateTime.Now.AddHours(-1));
        var result = pastTime.LaterThanNow();
        Assert.False(result.IsSuccess);
    }
}