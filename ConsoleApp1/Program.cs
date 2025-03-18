// See https://aka.ms/new-console-template for more information

using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

var locationType = LocationType.Outside;
var locationName = LocationName.Create("Meadows").Unwrap();
var locationCapacity = LocationCapacity.Create(100).Unwrap();
var location = Location.CreateLocation(locationType, locationName, locationCapacity).Unwrap();

var eventType = EventType.Public;
var eventStartDate = new EventTime(new DateTime(2026, 01, 01));
var eventEndDate = new EventTime(new DateTime(2026, 01, 01));
var eventResult = Event.CreateEvent(location, eventType, eventStartDate, eventEndDate);
var newEvent = eventResult.Unwrap();

var eventTitle = EventTitle.CreateEventTitle("imaginal disk event");
var eventDesc = EventDescription.CreateEventDescription("imaginal disk event description");


newEvent.ChangeTitle(eventTitle.Unwrap());
newEvent.ChangeDescription(eventDesc.Unwrap());


var guestName = GuestName.Create("hello", "hello").Unwrap();
var guestEmail = GuestVIAEmail.Create("abv@via.dk").Unwrap();

var validUrl = new Uri("https://example.com/profile.jpg");
var guestImage = GuestImageUrl.Create(validUrl).Unwrap();

var guest = Guest.Create(guestName, guestEmail, guestImage, null).Unwrap();
newEvent.ChangeEventStatusToActive();

var result = newEvent.RegisterGuestToEvent(guest);
Console.WriteLine(result.IsSuccess);


