using System.Threading.Tasks;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Application.CommandDispatching.Features;
using EventAssociation.Core.Tools.OperationResult;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Features.Guest.Register
{
    public class RegisterANewGuestHandlerTests
    {
        private readonly FakeGuestRepository guestRepository;
        private readonly FakeUnitOfWork unitOfWork;
        private readonly FakeEmailChecker emailChecker;
        private readonly RegisterANewGuestHandler handler;

        public RegisterANewGuestHandlerTests()
        {
            emailChecker = new FakeEmailChecker();
            guestRepository = new FakeGuestRepository(emailChecker);
            unitOfWork = new FakeUnitOfWork();
            handler = new RegisterANewGuestHandler(guestRepository, unitOfWork, emailChecker);
        }

        [Fact]
        public async Task HandleAsync_Should_Succeed_When_Guest_Is_Valid()
        {
            // Arrange
            var command = RegisterANewGuestCommand.Create("New", "Guest", "123123@via.dk", "https://example.com/new.jpg").Unwrap();

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.True(result.IsSuccess);
        }
        
    }
}
