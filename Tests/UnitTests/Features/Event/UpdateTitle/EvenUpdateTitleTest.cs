using System.Diagnostics;

namespace UnitTests.Features.Event.CreateEvent
{
    using Xunit;
    using EventAssociation.Core.Domain.Aggregates.Events.Values;
    using EventAssociation.Core.Tools.OperationResult;

    public class EventTitleTests
    {
        [Fact]
        public void EventTitle_ShouldSuccessfullySetTitle()
        {
            // Arrange
            var validTitle = "Valid Event Title";

            // Act
            var result = new EventTitle(validTitle);
            

            // Assert
            Assert.Equal(validTitle, result.Title);
        }

        [Fact]
        public void EventTitle_TitleIsNull_WhenEmptyTitleIsProvided()
        {
            // Arrange
            var emptyTitle = "";

            // Act
            var act = new EventTitle(emptyTitle);
            
            //Assert
            Assert.Null(act.Title);        

        }

        [Fact]
        public void EventTitle_TitleIsNull_WhenTitleIsTooShort()
        {
            // Arrange
            var shortTitle = "Hi"; // 2 characters

            // Act
            var act = new EventTitle(shortTitle);
            
            //Assert
            Assert.Null(act.Title);  
        }

        [Fact]
        public void EventTitle_TitleIsNull_WhenTitleIsTooLong()
        {
            // Arrange
            var longTitle = new string('A', 76); // 76 characters

            // Act
            var act = new EventTitle(longTitle);
            
            //Assert
            Assert.Null(act.Title);  
        }

        [Fact]
        public void EventTitle_ShouldInitializeCorrectly_WhenTitleIsExactlyThreeCharacters()
        {
            // Arrange
            var validTitle = "Hey"; // 3 characters

            // Act
            var eventTitle = new EventTitle(validTitle);

            // Assert
            Assert.Equal(validTitle, eventTitle.Title);
        }

        [Fact]
        public void EventTitle_ShouldInitializeCorrectly_WhenTitleIsExactlySeventyFiveCharacters()
        {
            // Arrange
            var validTitle = new string('A', 75); // 75 characters

            // Act
            var eventTitle = new EventTitle(validTitle);

            // Assert
            Assert.Equal(validTitle, eventTitle.Title);
        }
    }
}
