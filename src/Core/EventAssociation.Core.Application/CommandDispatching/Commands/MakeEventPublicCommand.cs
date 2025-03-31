using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class MakeEventPublicCommand : ICommand
{
    internal EventId Id { get; }

    private MakeEventPublicCommand(EventId id)
    {
        Id = id;
    }

    public static Result<MakeEventPublicCommand> Create(string id)
    {
        var validId = EventId.Create(id);
        return validId.IsSuccess
            ? Result<MakeEventPublicCommand>.Ok(new MakeEventPublicCommand(validId.Unwrap()))
            : Result<MakeEventPublicCommand>.Err(validId.UnwrapErr().ToArray());
    }
}