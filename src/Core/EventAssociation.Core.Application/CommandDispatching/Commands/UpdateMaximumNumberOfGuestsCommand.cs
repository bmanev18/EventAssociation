using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class UpdateMaximumNumberOfGuestsCommand
{
    internal EventId Id { get; }
    internal EventMaxParticipants MaxParticipants { get; }


    private UpdateMaximumNumberOfGuestsCommand(EventId id, EventMaxParticipants maxParticipants)
    {
        Id = id;
        MaxParticipants = maxParticipants;
    }

    public static Result<UpdateMaximumNumberOfGuestsCommand> Create(int maxParticipants, string Id)
    {
        var errors = new List<Error>();
        var idResult = EventId.Create(Id);
        var maxParticipantsResult = EventMaxParticipants.Create(maxParticipants);
       
        if (!idResult.IsSuccess) {
            errors.AddRange(idResult.UnwrapErr());
        }
        else if (!maxParticipantsResult.IsSuccess) {
            errors.AddRange(maxParticipantsResult.UnwrapErr());
        }

        if (errors.Any())
        {
            return Result<UpdateMaximumNumberOfGuestsCommand>.Err();
        }
        
        var command = new UpdateMaximumNumberOfGuestsCommand(idResult.Unwrap(), maxParticipantsResult.Unwrap());
        return Result<UpdateMaximumNumberOfGuestsCommand>.Ok(command);

    }

}