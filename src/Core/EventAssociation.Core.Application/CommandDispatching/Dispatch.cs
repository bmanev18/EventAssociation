using EventAssociation.Core.Tools.OperationResult;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventAssociation.Core.Application.CommandDispatching;

public class Dispatch : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ICommandDispatcher>? _logger;

    public Dispatch(IServiceProvider serviceProvider, ILogger<ICommandDispatcher>? logger = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler == null)
        {
            var errorMsg = $"No handler found for command {typeof(TCommand).Name}";
            _logger?.LogError(errorMsg);
            return Result<None>.Err(new Error("100", errorMsg));
        }

        return await handler.HandleAsync(command);
    }
}
