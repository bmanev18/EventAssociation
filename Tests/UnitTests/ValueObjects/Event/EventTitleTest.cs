using EventAssociation.Core.Domain.Aggregates.Events.Values;

namespace UnitTests.ValueObjects.Event;

public class EventTitleTest
{
    [Fact]
        public void EventTitle_ShouldSuccessfullySetTitle()
        {
            // Arrange
            var validTitle = "Valid Event Title";
        
            // Act
            var result = EventTitle.CreateEventTitle(validTitle);
            
        
            // Assert
            Assert.Equal(validTitle, result.Unwrap().Title);
        }
        
        [Fact]
        public void EventTitle_TitleIsNull_WhenEmptyTitleIsProvided()
        {
            // Arrange
            var emptyTitle = "";
        
            // Act
            var act = EventTitle.CreateEventTitle(emptyTitle);
            
            //Assert
            Assert.Single(act.UnwrapErr());
            Assert.Equal("Title can't be empty.",act.UnwrapErr().First().Message);

        }
        
        [Fact]
        public void EventTitle_TitleIsNull_WhenTitleIsTooShort()
        {
            // Arrange
            var shortTitle = "Hi"; // 2 characters
        
            // Act
            var act = EventTitle.CreateEventTitle(shortTitle);
            
            //Assert
            Assert.Single(act.UnwrapErr());
            Assert.Equal("Title must be at least 3 characters.",act.UnwrapErr().First().Message); 
        }
        
        [Fact]
        public void EventTitle_TitleIsNull_WhenTitleIsTooLong()
        {
            // Arrange
            var longTitle = new string('A', 76); // 76 characters
        
            // Act
            var act = EventTitle.CreateEventTitle(longTitle);
            
            //Assert
            Assert.Single(act.UnwrapErr());
            Assert.Equal("Title must be bellow 75 characters.",act.UnwrapErr().First().Message); 
        }
        
        [Theory]
        [InlineData("hey")]                 //Title is 3 characters
        [MemberData(nameof(return75LengthData))] //Title is 75 characters
        public void EventTitle_ShouldInitializeCorrectly_TitleIsWithinParameters(String input)
        {
            // Arrange
            // Act
            var eventTitle = EventTitle.CreateEventTitle(input);
        
            // Assert
            Assert.Equal(input, eventTitle.Unwrap().Title);
        }
        
        public static IEnumerable<object[]> return75LengthData()
        {
            yield return new object[] { new string('A', 75) };
        }
}