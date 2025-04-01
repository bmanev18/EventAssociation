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

        var handlers = new List<object> { handlerMock.Object };
        var dispatcher = new Dispatch(handlers);

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
        var dispatcher = new Dispatch(new List<object>());

        // Act
        var result = await dispatcher.DispatchAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("100", result.UnwrapErr().First().Code);
        Assert.Equal($"No handler found for command {typeof(DeclineInvitationCommand).Name}", result.UnwrapErr()[0].Message);
    }
    
    [Fact]
    public async Task DispatchAsync_CallsOnlyTheCorrectHandler()
    {
        // Arrange
        var declineCommand = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        var acceptCommand = AcceptInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        
        var declineHandlerMock = new Mock<ICommandHandler<DeclineInvitationCommand>>();
        declineHandlerMock
            .Setup(h => h.HandleAsync(declineCommand))
            .ReturnsAsync(Result<None>.Ok(None.Value));

        var acceptHandlerMock = new Mock<ICommandHandler<AcceptInvitationCommand>>();
        acceptHandlerMock
            .Setup(h => h.HandleAsync(acceptCommand))
            .ReturnsAsync(Result<None>.Ok(None.Value));
        
        var handlers = new List<object> { declineHandlerMock.Object, acceptHandlerMock.Object };
        var dispatcher = new Dispatch(handlers);

        // Act
        var result = await dispatcher.DispatchAsync(declineCommand);

        // Assert
        Assert.True(result.IsSuccess);
        
        declineHandlerMock.Verify(h => h.HandleAsync(declineCommand), Times.Once);
        acceptHandlerMock.Verify(h => h.HandleAsync(It.IsAny<AcceptInvitationCommand>()), Times.Never);
        declineHandlerMock.Verify(h => h.HandleAsync(It.IsAny<DeclineInvitationCommand>()), Times.Once);
    }
}