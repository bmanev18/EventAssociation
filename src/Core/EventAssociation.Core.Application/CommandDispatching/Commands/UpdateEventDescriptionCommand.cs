using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventDescriptionCommand
{
    internal EventId Id { get; }
    internal EventDescription Description { get; }

    private UpdateEventDescriptionCommand(EventDescription description)
    {
        Description = description;
    }

    public static Result<UpdateEventDescriptionCommand> Create(string id, string description)
    {
        // new EventId();
        var result = EventDescription.CreateEventDescription(description);
        return !result.IsSuccess
            ? Result<UpdateEventDescriptionCommand>.Err(result.UnwrapErr().ToArray())
            : Result<UpdateEventDescriptionCommand>.Ok(new UpdateEventDescriptionCommand(result.Unwrap()));
    }
}