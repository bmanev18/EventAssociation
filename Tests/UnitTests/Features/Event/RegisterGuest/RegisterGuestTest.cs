using Xunit;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Tools.OperationResult;
using System;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using UnitTests.Fakes;

namespace EventAssociation.Tests;

    public class EventTests
    {
        private Event CreateMockEvent(EventType eventType, DateTime? startTime = null, DateTime? endTime = null)
        {
            var location = Location.CreateLocation(
                LocationType.Outside,
                LocationName.Create("Meadows").Unwrap(),
                LocationCapacity.Create(100).Unwrap()
            ).Unwrap();

            var eventStartDate = new EventTime(startTime ?? DateTime.UtcNow.AddDays(1)); // Default to future event
            var eventEndDate = new EventTime(endTime ?? DateTime.UtcNow.AddDays(2));

            var eventResult = Event.CreateEvent(location, eventType, eventStartDate, eventEndDate);
            var newEvent = eventResult.Unwrap();

            newEvent.ChangeTitle(EventTitle.CreateEventTitle("Test Event").Unwrap());
            newEvent.ChangeDescription(EventDescription.CreateEventDescription("Test Description").Unwrap());

            return newEvent;
        }

        private Guest CreateMockGuest(string firstName, string lastName, string email, string imageUrl)
        {
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var guestName = GuestName.Create(firstName, lastName).Unwrap();
            var guestEmail = GuestVIAEmail.Create(email).Unwrap();
            var guestImage = GuestImageUrl.Create(new Uri(imageUrl)).Unwrap();
            return Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();
        }


        [Fact]
        public void RegisterGuest_Success_Scenario()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var mockEvent = CreateMockEvent(EventType.Public);
            var guest = CreateMockGuest("Noel", "Gallagher", "abv@via.dk", "https://example.com/profile.jpg");
            // Set the event to active
            var result1 = mockEvent.ChangeEventStatusToActive();
            Assert.True(result1.IsSuccess);


            // Act
            var result = mockEvent.RegisterGuestToEvent(guest);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void RegisterGuest_Failure_EventNotActive()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var mockEvent = CreateMockEvent(EventType.Public);
            var guestName = GuestName.Create("Test", "Testov").Unwrap();
            var guestEmail = GuestVIAEmail.Create("abv@via.dk").Unwrap();

            var validUrl = new Uri("https://example.com/profile.jpg");
            var guestImage = GuestImageUrl.Create(validUrl).Unwrap();

            var guest = Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();

            // Act: Try registering without activating the event
            var result = mockEvent.RegisterGuestToEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cannot register an inactive event", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void RegisterGuest_Failure_NoMoreRoom()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var mockEvent = CreateMockEvent(EventType.Public);

            var guest1 = CreateMockGuest("John", "Doe", "123123@via.dk", "https://example.com/john.jpg");
            var guest2 = CreateMockGuest("Paul", "Mccartney", "000000@via.dk", "https://example.com/john.jpg");
            var guest3 = CreateMockGuest("isaac", "Asimov", "111111@via.dk", "https://example.com/john.jpg");
            var guest4 = CreateMockGuest("Stevie", "Wonder", "222222@via.dk", "https://example.com/john.jpg");
            var guest5 = CreateMockGuest("Drake", "Drake", "333333@via.dk", "https://example.com/john.jpg");
            var guest6 = CreateMockGuest("Sally", "Sally", "444444@via.dk", "https://example.com/john.jpg");

            // Set the event to active and set capacity to 1
            mockEvent.ChangeEventStatusToActive();

            // Register first guests
            mockEvent.RegisterGuestToEvent(guest1);
            mockEvent.RegisterGuestToEvent(guest2);
            mockEvent.RegisterGuestToEvent(guest3);
            mockEvent.RegisterGuestToEvent(guest4);
            mockEvent.RegisterGuestToEvent(guest5);


            // Act: should fail due to no capacity
            var result = mockEvent.RegisterGuestToEvent(guest6);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("There are no available spots in the guest list", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void RegisterGuest_Failure_CannotJoinPrivateEvent()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var mockEvent = CreateMockEvent(EventType.Private);
            var guestName = GuestName.Create("Test", "Guest").Unwrap();
            var guestEmail = GuestVIAEmail.Create("test@via.dk").Unwrap();

            var validUrl = new Uri("https://example.com/profile.jpg");
            var guestImage = GuestImageUrl.Create(validUrl).Unwrap();

            var guest = Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();

            // Set the event to private
            mockEvent.ChangeEventStatusToActive().Unwrap();

            // Act: Try registering for a private event
            var result = mockEvent.RegisterGuestToEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("You cannot join a private event. Only public events can be joined.",
                result.UnwrapErr().First().Message);
        }

        [Fact]
        public void RegisterGuest_Failure_GuestAlreadyParticipating()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var mockEvent = CreateMockEvent(EventType.Public);
            var guestName = GuestName.Create("Test", "Guest").Unwrap();
            var guestEmail = GuestVIAEmail.Create("test@via.dk").Unwrap();

            var validUrl = new Uri("https://example.com/profile.jpg");
            var guestImage = GuestImageUrl.Create(validUrl).Unwrap();

            var guest = Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();

            // Set the event to active
            mockEvent.ChangeEventStatusToActive().Unwrap();

            // Register the guest
            mockEvent.RegisterGuestToEvent(guest).Unwrap();

            // Act: Try registering the same guest again (should fail)
            var result = mockEvent.RegisterGuestToEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("You are already registered to attend this event.", result.UnwrapErr().First().Message);
        }

        [Fact]
        public void RegisterGuest_Failure_EventAlreadyStarted()
        {
            // Arrange: Create an event that started in the past
            var pastStartTime = DateTime.UtcNow.AddHours(-1); // Event started 1 hour ago
            var futureEndTime = DateTime.UtcNow.AddHours(2);
            var mockEvent = CreateMockEvent(EventType.Public, pastStartTime, futureEndTime);
            mockEvent.ChangeEventStatusToActive(); // Activate the event

            var guest = CreateGuest();

            // Act
            var result = mockEvent.RegisterGuestToEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cannot join an event that has already started.", result.UnwrapErr().First().Message);
        }

        private Guest CreateGuest()
        {
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var guestName = GuestName.Create("Test", "User").Unwrap();
            var guestEmail = GuestVIAEmail.Create("test@via.dk").Unwrap();
            var guestImage = GuestImageUrl.Create(new Uri("https://example.com/profile.jpg")).Unwrap();

            return Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();
        }
    }

