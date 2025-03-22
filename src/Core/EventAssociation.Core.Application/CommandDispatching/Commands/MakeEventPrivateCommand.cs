using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class MakeEventPrivateCommand
{
    internal EventId Id { get; }

    private MakeEventPrivateCommand(EventId id)
    {
        Id = id;
    }

    public static Result<MakeEventPrivateCommand> Create(string id)
    {
        var validId = EventId.Create(id);
        return validId.IsSuccess
            ? Result<MakeEventPrivateCommand>.Ok(new MakeEventPrivateCommand(validId.Unwrap()))
            : Result<MakeEventPrivateCommand>.Err(validId.UnwrapErr().ToArray());
    }
}