using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Tools.OperationResult;
using Moq;

namespace UnitTests.Common;

public class CommandDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_CallsCorrectHandler()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        var handlerMock = new Mock<ICommandHandler<DeclineInvitationCommand>>();
        handlerMock
            .Setup(h => h.HandleAsync(command))
            .ReturnsAsync(Result<None>.Ok(None.Value));

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICommandHandler<DeclineInvitationCommand>)))
            .Returns(handlerMock.Object);

        var dispatcher = new Dispatch(serviceProviderMock.Object);

        // Act
        var result = await dispatcher.DispatchAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        handlerMock.Verify(h => h.HandleAsync(command), Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_NoHandler_ReturnsError()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICommandHandler<DeclineInvitationCommand>)))
            .Returns(null); // Simulate handler not registered

        var dispatcher = new Dispatch(serviceProviderMock.Object);

        // Act
        var result = await dispatcher.DispatchAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        var error = result.UnwrapErr().First();
        Assert.Equal("100", error.Code);
        Assert.Equal($"No handler found for command {typeof(DeclineInvitationCommand).Name}", error.Message);
    }

    [Fact]
    public async Task DispatchAsync_CallsOnlyTheCorrectHandler()
    {
        // Arrange
        var declineCommand = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        var acceptCommand = AcceptInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();

        var declineHandlerMock = new Mock<ICommandHandler<DeclineInvitationCommand>>();
        var acceptHandlerMock = new Mock<ICommandHandler<AcceptInvitationCommand>>();

        declineHandlerMock
            .Setup(h => h.HandleAsync(declineCommand))
            .ReturnsAsync(Result<None>.Ok(None.Value));

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICommandHandler<DeclineInvitationCommand>)))
            .Returns(declineHandlerMock.Object);
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICommandHandler<AcceptInvitationCommand>)))
            .Returns(acceptHandlerMock.Object);

        var dispatcher = new Dispatch(serviceProviderMock.Object);

        // Act
        var result = await dispatcher.DispatchAsync(declineCommand);

        // Assert
        Assert.True(result.IsSuccess);
        declineHandlerMock.Verify(h => h.HandleAsync(declineCommand), Times.Once);
        acceptHandlerMock.Verify(h => h.HandleAsync(It.IsAny<AcceptInvitationCommand>()), Times.Never);
    }
}