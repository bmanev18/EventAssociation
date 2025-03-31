using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateEventTitleCommand : ICommand
{
    internal EventId Id { get; }
    internal EventTitle Title { get; }

    private UpdateEventTitleCommand(EventId id, EventTitle title)
    {
        Id = id;
        Title = title;
    }

    public static Result<UpdateEventTitleCommand> Create(string id, string title)
    {
        var result = EventTitle.CreateEventTitle(title);
        var validId = EventId.Create(id);
        
        var errors = new List<Error>();
        if (!result.IsSuccess)
        {
            errors.AddRange(result.UnwrapErr());
        }
        else if (!validId.IsSuccess)
        {
            errors.AddRange(validId.UnwrapErr());
        }else
        {
            return Result<UpdateEventTitleCommand>.Ok(new UpdateEventTitleCommand(validId.Unwrap(), result.Unwrap()));
        }
        
        return Result<UpdateEventTitleCommand>.Err(errors.ToArray());
    }
}