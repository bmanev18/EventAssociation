using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;


public class UpdateEventStatusToActiveCommand : ICommand
{
    internal EventId Id { get; }

    private UpdateEventStatusToActiveCommand(EventId id)
    {
        Id = id;
    }

    public static Result<UpdateEventStatusToActiveCommand> Create(string id)
    {
        var validId = EventId.Create(id);
        return validId.IsSuccess
            ? Result<UpdateEventStatusToActiveCommand>.Ok(new UpdateEventStatusToActiveCommand(validId.Unwrap()))
            : Result<UpdateEventStatusToActiveCommand>.Err(validId.UnwrapErr().ToArray());
    }
}