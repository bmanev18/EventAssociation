using System.Diagnostics;
using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.Logging;

namespace EventAssociation.Core.Application.CommandDispatching;

public class CommandExecutionTimerDispatcher : ICommandDispatcher
{
    
    private readonly ICommandDispatcher _next;
    private readonly ILogger<CommandExecutionTimerDispatcher>? _logger;

    public CommandExecutionTimerDispatcher(ICommandDispatcher next)
    {
        _next = next;
    }

    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        Result<None> result = await _next.DispatchAsync(command);
        
        stopwatch.Stop();
        double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;

        string commandName = typeof(TCommand).Name;
        string message = $"Command {commandName} executed in {elapsedMs} ms.";

        if (_logger != null)
        {
            _logger.LogInformation(message);
        }
        else
        {
            Console.WriteLine(message);
        }
        
        return result;
    }
}