using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Common;

public class CommandExecutionTimerDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_CallsNextDispatcher()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        
        // Mock the next dispatcher
        var nextDispatcherMock = new Mock<ICommandDispatcher>();
        nextDispatcherMock
            .Setup(d => d.DispatchAsync(command))
            .ReturnsAsync(Result<None>.Ok(None.Value));

        var loggerMock = new Mock<ILogger<CommandExecutionTimerDispatcher>>();

        var timerDispatcher = new CommandExecutionTimerDispatcher(nextDispatcherMock.Object, loggerMock.Object);

        // Act
        var result = await timerDispatcher.DispatchAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        
        nextDispatcherMock.Verify(d => d.DispatchAsync(command), Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_LogsExecutionTime()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        
        var nextDispatcherMock = new Mock<ICommandDispatcher>();
        nextDispatcherMock
            .Setup(d => d.DispatchAsync(command))
            .ReturnsAsync(Result<None>.Ok(None.Value));

        var loggerMock = new Mock<ILogger<CommandExecutionTimerDispatcher>>();

        var timerDispatcher = new CommandExecutionTimerDispatcher(nextDispatcherMock.Object, loggerMock.Object);

        // Act
        await timerDispatcher.DispatchAsync(command);

        // Assert: Verify logger was called with correct format
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Command {typeof(DeclineInvitationCommand).Name} executed in")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task DispatchAsync_ReturnsErrorIfNextDispatcherFails()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();

        var nextDispatcherMock = new Mock<ICommandDispatcher>();
        nextDispatcherMock
            .Setup(d => d.DispatchAsync(command))
            .ReturnsAsync(Result<None>.Err(new Error("500", "Dispatcher failed")));

        var loggerMock = new Mock<ILogger<CommandExecutionTimerDispatcher>>();

        var timerDispatcher = new CommandExecutionTimerDispatcher(nextDispatcherMock.Object, loggerMock.Object);

        // Act
        var result = await timerDispatcher.DispatchAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("500", result.UnwrapErr()[0].Code);
        Assert.Equal("Dispatcher failed", result.UnwrapErr()[0].Message);
    }
}