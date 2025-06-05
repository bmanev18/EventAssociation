using EventAssociation.Core.QueryContracts.Queries;
using EventAssociation.Infrastructure.EfcQueries.Models;
using EventAssociation.Infrastructure.EfcQueries.Queries;
using EventAssociation.Infrastructure.EfcQueries.SeedFactories;
using IntegrationTests.Shared;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EventAssociation.Infrastructure.EfcQueries.Tests.Queries;

public class EventDetailsHandlerTests
{
    [Fact] // Removed Skip attribute
    public async Task HandleAsync_ReturnsCorrectlyCategorizedEvents()
    {
        // Arrange
        var context = Setup.SetupContext();
        var eventsData = Setup.Seed(context);
        
        var handler = new EventDetailsHandler(context);
        var query = new EventsOverview.Query();

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        
        // Check draft events (5 events in Events.json with status "draft")
        Assert.Equal(5, result.draftEvents.Count);
        Assert.Contains(result.draftEvents, e => e.Title == "DnD introductions!");
        Assert.Contains(result.draftEvents, e => e.Title == "Whiskey Tasting");
        Assert.Contains(result.draftEvents, e => e.Title == "Card Stacking.");
        Assert.Contains(result.draftEvents, e => e.Title == "Soap Carving");
        Assert.Contains(result.draftEvents, e => e.Title == "Extreme Ironing");
        
        // Check ready events (4 events in Events.json with status "ready")
        Assert.Equal(4, result.readyEvents.Count);
        Assert.Contains(result.readyEvents, e => e.Title == "Chess and beer");
        Assert.Contains(result.readyEvents, e => e.Title == "Beer Tasting!");
        Assert.Contains(result.readyEvents, e => e.Title == "Art Exhibition");
        Assert.Contains(result.readyEvents, e => e.Title == "Juggling");
        
        // Check cancelled events (2 events in Events.json with status "cancelled")
        Assert.Equal(2, result.cancelledEvents.Count);
        Assert.Contains(result.cancelledEvents, e => e.Title == "Learn to knit!");
        Assert.Contains(result.cancelledEvents, e => e.Title == "Origami Introduction");
    }

    
}