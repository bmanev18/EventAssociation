﻿using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;

namespace UnitTests.Features.Event.CreateEvent;
using Xunit;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;

public class CreateEventAggregateTest
{
    [Fact]
    public void CreateEvent_ShouldReturnSuccessResult()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();

        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();

        // Act
        var result = Event.CreateEvent(location, EventType.Private, null, null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Unwrap());
    }

    [Fact]
    public void CreateEvent_ShouldInitializeFieldsCorrectly()
    {
        // Arrange
        var locationName = LocationName.Create("Meadows").Unwrap();
        var locationCapacity = LocationCapacity.Create(20).Unwrap();

        var location = Location.CreateLocation(LocationType.Outside, locationName, locationCapacity).Unwrap();

        // Act
        var createdEvent = Event.CreateEvent(location, EventType.Private, null, null).Unwrap();

        // Assert
        Assert.Equal(EventTitle.CreateEventTitle("Working Title").Unwrap(),createdEvent.Title);
        Assert.Equal(EventDescription.CreateEventDescription("").Unwrap(), createdEvent.Description);
        Assert.Equal(EventMaxParticipants.Create(5).Unwrap(), createdEvent.MaxParticipants);
        Assert.Equal(EventType.Private, createdEvent.Type);
        Assert.Equal(EventStatus.Draft, createdEvent.Status);
    }
    

     
    
}