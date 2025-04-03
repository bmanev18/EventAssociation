using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventAssociation.Core.Application.CommandDispatching;

public class Dispatch : ICommandDispatcher
{
    private readonly Dictionary<Type, object> _handlers;
    private readonly ILogger<ICommandDispatcher>? _logger;

    public Dispatch(IEnumerable<object> handlers, ILogger<ICommandDispatcher>? logger = null)
    {
        _logger = logger;
        _handlers = handlers.ToDictionary(handler => handler.GetType().GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            .GetGenericArguments()[0], handler => handler);
    }

    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        if (_handlers.TryGetValue(typeof(TCommand), out var handlerObj) &&
            handlerObj is ICommandHandler<TCommand> handler)
        {
            return await handler.HandleAsync(command);
        }

        var errorMsg = $"No handler found for command {typeof(TCommand).Name}";
        _logger?.LogError(errorMsg);
        return Result<None>.Err(new Error("100", errorMsg));
    }
}