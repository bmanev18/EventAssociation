using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching;

public interface ICommandDispatcher
{
    public Task<Result<None>> DispatchAsync<TCommand>(TCommand command);
}