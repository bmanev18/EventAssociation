using EventAssociation.Core.Application.CommandDispatching.Features;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventStatusToReadyCommand : ICommand
{
    internal EventId Id { get; }

    private UpdateEventStatusToReadyCommand(EventId id)
    {
        Id = id;
    }

    public static Result<UpdateEventStatusToReadyCommand> Create(string id)
    {
        var validId = EventId.Create(id);
        return validId.IsSuccess
            ? Result<UpdateEventStatusToReadyCommand>.Ok(new UpdateEventStatusToReadyCommand(validId.Unwrap()))
            : Result<UpdateEventStatusToReadyCommand>.Err(validId.UnwrapErr().ToArray());
    }
}