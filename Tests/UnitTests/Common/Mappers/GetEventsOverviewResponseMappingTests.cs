using WebAPI.Endpoints;
using WebAPI.Mappers;
using EventAssociation.Core.QueryContracts.Queries;

namespace UnitTests.Mappers
{
    public class GetEventsOverviewResponseMappingTests
    {
        [Fact]
        public void Map_ResponseToAnswer_ShouldMapCorrectly()
        {
            // Arrange
            var response = new GetEventsOverviewResponse(
                ReadyEvents: new List<GetEventsOverviewResponse.EventInfo>
                {
                    new("Ready Event 1"),
                    new("Ready Event 2")
                },
                DraftEvents: new List<GetEventsOverviewResponse.EventInfo>
                {
                    new("Draft Event 1")
                },
                CancelledEvents: new List<GetEventsOverviewResponse.EventInfo>
                {
                    new("Cancelled Event 1"),
                    new("Cancelled Event 2"),
                    new("Cancelled Event 3")
                }
            );

            var mapper = new GetEventsOverviewResponseMapping();

            // Act
            EventsOverview.Answer result = mapper.Map(response);

            // Assert
            Assert.Equal(2, result.readyEvents.Count);
            Assert.Equal("Ready Event 1", result.readyEvents[0].Title);
            Assert.Equal("Ready Event 2", result.readyEvents[1].Title);

            Assert.Single(result.draftEvents);
            Assert.Equal("Draft Event 1", result.draftEvents[0].Title);

            Assert.Equal(3, result.cancelledEvents.Count);
            Assert.Equal("Cancelled Event 1", result.cancelledEvents[0].Title);
            Assert.Equal("Cancelled Event 2", result.cancelledEvents[1].Title);
            Assert.Equal("Cancelled Event 3", result.cancelledEvents[2].Title);
        }
    }
}