using System.Diagnostics;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.Logging;

namespace EventAssociation.Core.Application.CommandDispatching;

public class CommandExecutionTimerDispatcher : ICommandDispatcher
{
    
    private readonly ICommandDispatcher _next;
    private readonly ILogger<CommandExecutionTimerDispatcher> _logger;

    public CommandExecutionTimerDispatcher(ICommandDispatcher next, ILogger<CommandExecutionTimerDispatcher> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Result<None> result = await _next.DispatchAsync(command);
        stopwatch.Stop();

        _logger.LogInformation("Command {CommandName} executed in {ElapsedMs} ms.", typeof(TCommand).Name, stopwatch.Elapsed.TotalMilliseconds);
        
        return result;
    }
}