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
    [Fact(Skip = "Still figuring out")]
    public async Task HandleAsync_ReturnsCorrectlyCategorizedEvents()
    {
        // Arrange
        var context = Setup.SetupContext();
        // var eventsData = Setup.Seed(context);
        
        var handler = new EventDetailsHandler(context);
        var query = new EventsOverview.Query();

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Collection(result.draftEvents,
            e => Assert.Equal("Draft Event 1", e.Title),
            e => Assert.Equal("Draft Event 2", e.Title));
        Assert.Collection(result.readyEvents,
            e => Assert.Equal("Ready Event A", e.Title),
            e => Assert.Equal("Ready Event B", e.Title),
            e => Assert.Equal("Ready Event C", e.Title));
        Assert.Collection(result.cancelledEvents,
            e => Assert.Equal("Cancelled Event X", e.Title));
    }

    
}