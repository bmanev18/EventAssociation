// See https://aka.ms/new-console-template for more information

using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Tools.OperationResult;

// Console.WriteLine("Hello, World!");
//
// var success = Result<int>.Ok(42);
// Console.WriteLine(success.IsSuccess); // true
// Console.WriteLine(success.Unwrap());  // 42
//
// var errorResult = Result<int>.Err(
//     new Error("E001", "Invalid input"),
//     new Error("E002", "Not found")
// );
// Console.WriteLine(errorResult.IsSuccess); // false
// Console.WriteLine(errorResult.UnwrapErr().Count); // 2
//
//
// var  noneResult = Result<None>.Ok(None.Value);
//
// var title1 = new EventTitle("Valid Event Title");
// Console.WriteLine(title1.Title);
// Console.WriteLine(success.IsSuccess); // true

var locationCapacity = LocationCapacity.Create(100).Unwrap();
var locationName = LocationName.Create("Meadows").Unwrap();
var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();
var event_ = Event.CreateEvent(location, EventType.Private).Unwrap();
var test = event_.UpdateMaxNumberOfParticipants(70);
Console.WriteLine(test.IsSuccess.ToString());

event_.ChangeEventStatusToActive();
var result = event_.UpdateMaxNumberOfParticipants(60);
Console.WriteLine(result.UnwrapErr());

var eventTitle = EventTitle.CreateEventTitle("ndand");
Console.WriteLine(eventTitle.HasErrors());