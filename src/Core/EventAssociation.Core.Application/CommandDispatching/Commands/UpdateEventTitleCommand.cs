using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventTitleCommand
{
    internal EventId Id { get; }
    internal EventTitle Title { get; }

    private UpdateEventTitleCommand(EventTitle title)
    {
        Title = title;
    }

    public static Result<UpdateEventTitleCommand> Create(string id, string title)
    {
        // new EventId();
        var result = EventTitle.CreateEventTitle(title);
        return !result.IsSuccess
            ? Result<UpdateEventTitleCommand>.Err(result.UnwrapErr().ToArray())
            : Result<UpdateEventTitleCommand>.Ok(new UpdateEventTitleCommand(result.Unwrap()));
    }
}