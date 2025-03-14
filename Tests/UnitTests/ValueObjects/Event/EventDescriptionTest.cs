using EventAssociation.Core.Domain.Aggregates.Events.Values;

namespace UnitTests.Features.Event.UpdateDescription;

public class EventDescriptionTest
{
    [Fact]
    public void EventDescription_ShouldSuccessfullySetTitle()
    {
        // Arrange
        var validDescription = "Valid Event Description";

        // Act
        var result = EventDescription.CreateEventDescription(validDescription);


        // Assert
        Assert.Equal(validDescription, result.Unwrap().Description);
    }

    [Fact]
    public void EventTitle_DescriptionIsSet_WhenEmptyDescriptionIsProvided()
    {
        // Arrange
        var emptyDescription = "";

        // Act
        var act =  EventDescription.CreateEventDescription(emptyDescription);

        //Assert
        Assert.Equal(emptyDescription, act.Unwrap().Description);
    }

    [Fact]
    public void EventTitle_DescriptionIsSet_WhenDescriptionIsTooShort()
    {
        // Arrange
        var shortDescription = "H"; // 1 characters

        // Act
        var act =  EventDescription.CreateEventDescription(shortDescription);

        //Assert
        Assert.Equal(shortDescription, act.Unwrap().Description);
    }

    [Fact]
    public void EventTitle_DescriptionIsNull_WhenDescriptionIsTooLong()
    {
        // Arrange
        var longDescription = new string('A', 251); // 76 characters
    
        // Act
        var act = EventDescription.CreateEventDescription(longDescription);
    
        //Assert
        Assert.Single(act.UnwrapErr());
        Assert.Equal("Description must be bellow 250 characters.", act.UnwrapErr().First().Message);
    }
    
    [Theory]
    [InlineData("h")] //Title is 1 characters
    [MemberData(nameof(return250LengthData))] //Title is 250 characters
    public void EventDescription_ShouldInitializeCorrectly_DescriptionIsWithinParameters(String input)
    {
        // Arrange
        // Act
        var eventDescription = EventDescription.CreateEventDescription(input);
    
        // Assert
        Assert.Equal(input, eventDescription.Unwrap().Description);
    }
    
    public static IEnumerable<object[]> return250LengthData()
    {
        yield return new object[] { new string('A', 250) };
    }
}