using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching;

public interface ICommandHandler<T>
{
    public Task<Result<None>> HandleAsync(T command);

}