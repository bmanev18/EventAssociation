using EventAssociation.Core.QueryContracts.Queries;
using EventAssociation.Infrastructure.EfcQueries.Models;
using EventAssociation.Infrastructure.EfcQueries.Queries;
using EventAssociation.Infrastructure.EfcQueries.SeedFactories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EventAssociation.Infrastructure.EfcQueries.Tests.Queries;

public class EventDetailsHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReturnsCorrectlyCategorizedEvents()
    {
        // Arrange
        var eventsData = EventSeedFactory.CreateEvents();

        var mockContext = new Mock<EventAssociationProductionContext>();
        var mockDbSet = MockDbContext.CreateDbSetMock(eventsData);
        mockContext.Setup(c => c.Events).Returns(mockDbSet.Object);

        var handler = new EventDetailsHandler(mockContext.Object);
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

    // Helper class to mock DbSet
    private static class MockDbContext
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
    }
}