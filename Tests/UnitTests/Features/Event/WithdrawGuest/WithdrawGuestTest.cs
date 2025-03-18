using Xunit;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Tools.OperationResult;
using System;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using UnitTests.Fakes;

namespace EventAssociation.Tests;

    public class WithdrawGuestTest
    {
        private Event CreateMockEvent()
        {
            var location = Location.CreateLocation(
                LocationType.Outside, 
                LocationName.Create("Meadows").Unwrap(), 
                LocationCapacity.Create(100).Unwrap()
            ).Unwrap();

            var eventStartDate = new EventTime(DateTime.UtcNow.AddDays(1));
            var eventEndDate = new EventTime(DateTime.UtcNow.AddDays(2));

            var eventResult = Event.CreateEvent(location, EventType.Public, eventStartDate, eventEndDate);
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
        public void WithdrawGuest_Fail_WhenEventIsNotActive()
        {
            // Arrange
            var mockEvent = CreateMockEvent(); // Event is not active
            var guest = CreateMockGuest("Noel", "Gallagher", "abv@via.dk", "https://example.com/profile.jpg");
            mockEvent.ChangeEventStatusToActive();
            mockEvent.RegisterGuestToEvent(guest); // Guest is registered

            // Act
            var result = mockEvent.WithdrawGuestFromAnEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("You cannot withdraw from an active event.", result.UnwrapErr().First().Message);
        }
        
        [Fact]
        public void WithdrawGuest_Success_WhenEventIsNotActive()
        {
            // Arrange
            var mockEvent = CreateMockEvent(); // Event is not active
            var guest = CreateMockGuest("Noel", "Gallagher", "abv@via.dk", "https://example.com/profile.jpg");
            mockEvent.ChangeEventStatusToActive();
            mockEvent.RegisterGuestToEvent(guest); // Guest is registered
            mockEvent.ChangeEventStatusToDraft();
            // Act
            var result = mockEvent.WithdrawGuestFromAnEvent(guest);

            // Assert
            Assert.True(result.IsSuccess);
        }
        
        
        [Fact]
        public void WithdrawGuest_Failure_WhenGuestNotRegistered()
        {
            // Arrange
            // Arrange
            var mockEvent = CreateMockEvent(); // Event is not active
            var guest = CreateMockGuest("Noel", "Gallagher", "abv@via.dk", "https://example.com/profile.jpg");
            mockEvent.ChangeEventStatusToActive();

            // Act
            var result = mockEvent.WithdrawGuestFromAnEvent(guest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("You are not registered to attend this event.", result.UnwrapErr().First().Message);
        }

     

    }

