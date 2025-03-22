using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventDescriptionCommand
{
    internal EventId Id { get; }
    internal EventDescription Description { get; }

    private UpdateEventDescriptionCommand(EventId id, EventDescription description)
    {
        Id = id;
        Description = description;
    }

    public static Result<UpdateEventDescriptionCommand> Create(string id, string description)
    {
        var result = EventDescription.CreateEventDescription(description);
        var validId = EventId.Create(id);

        var errors = new List<Error>();
        if (!result.IsSuccess)
        {
            errors.AddRange(result.UnwrapErr());
        }
        else if (!validId.IsSuccess)
        {
            errors.AddRange(validId.UnwrapErr());
        }
        else
        {
            return Result<UpdateEventDescriptionCommand>.Ok(
                new UpdateEventDescriptionCommand(validId.Unwrap(), result.Unwrap()));
        }

        return Result<UpdateEventDescriptionCommand>.Err(errors.ToArray());
    }
}