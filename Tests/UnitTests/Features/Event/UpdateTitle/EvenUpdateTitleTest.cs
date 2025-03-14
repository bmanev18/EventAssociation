using System.Diagnostics;

namespace UnitTests.Features.Event.CreateEvent
{
    using Xunit;
    using EventAssociation.Core.Domain.Aggregates.Events.Values;
    using EventAssociation.Core.Domain.Aggregates.Event;
    using EventAssociation.Core.Tools.OperationResult;

    public class EventTitleTests
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
        
        [Fact]
        public void ChangeTitle_ShouldReturnError_WhenEventIsActive()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var activeEvent = Event.CreateEvent(eventId).Unwrap();
            activeEvent.ChangeEventStatusToActive();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");
        
            // Act
            var result = activeEvent.ChangeTitle(newTitle.Unwrap());
        
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
            var newTitle = EventTitle.CreateEventTitle("New Event Title");
        
            // Act
            var result = cancelledEvent.ChangeTitle(newTitle.Unwrap());
        
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
            var newTitle = EventTitle.CreateEventTitle("New Event Title");
        
            // Act
            var result = readyEvent.ChangeTitle(newTitle.Unwrap());
        
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle.Unwrap(), readyEvent.Title);
            Assert.Equal(EventStatus.Draft, readyEvent.Status);
        }
        
        [Fact]
        public void ChangeTitle_ShouldUpdateTitle_WhenEventIsDraft()
        {
            // Arrange
            var eventId = new EventId(Guid.NewGuid());
            var draftEvent = Event.CreateEvent(eventId).Unwrap();
            draftEvent.ChangeEventStatusToDraft();
            var newTitle = EventTitle.CreateEventTitle("New Event Title");
        
            // Act
            var result = draftEvent.ChangeTitle(newTitle.Unwrap());
        
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle.Unwrap(), draftEvent.Title);
            Assert.Equal(EventStatus.Draft, draftEvent.Status);
        }
        
    }
}
