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

namespace EventAssociation.Tests
{

    public class EventTests
    {
        [Fact]
        public void RegisterGuest_Success_Scenario_S1()
        {
            // Arrange
            FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();
            var locationType = LocationType.Outside;
            var locationName = LocationName.Create("Meadows").Unwrap();
            var locationCapacity = LocationCapacity.Create(100).Unwrap();
            var location = Location.CreateLocation(locationType, locationName, locationCapacity).Unwrap();

            var eventType = EventType.Public;
            var eventStartDate = new EventTime(new DateTime(2026, 01, 01));
            var eventEndDate = new EventTime(new DateTime(2026, 01, 01));
            var eventResult = Event.CreateEvent(location, eventType, eventStartDate, eventEndDate);
            var newEvent = eventResult.Unwrap();

            var guestName = GuestName.Create("Noel", "Gallagher").Unwrap();
            var guestEmail = GuestVIAEmail.Create("abv@via.dk").Unwrap();

            var validUrl = new Uri("https://example.com/profile.jpg");
            var guestImage = GuestImageUrl.Create(validUrl).Unwrap();


            var guest = Guest.Create(guestName, guestEmail, guestImage, fakeEmailChecker).Unwrap();

            // Set the event to active
            newEvent.ChangeEventStatusToActive();

            // Act
            var result = newEvent.RegisterGuest(guest);

            // Assert
            Assert.True(result.IsSuccess);
        }

    }
}
