using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class CreateInvitationCommand
{
    internal InvitationEventId InvitationEventId { get;  }
    
    internal InvitationGuestId InvitationGuestId { get;  }


    private CreateInvitationCommand(InvitationEventId invitationEventId, InvitationGuestId invitationGuestId)
    {
        InvitationEventId = invitationEventId;
        InvitationGuestId = invitationGuestId;
    }

    public static Result<CreateInvitationCommand> Create(InvitationEventId invitationEventId, InvitationGuestId invitationGuestId)
    {
        var command = new CreateInvitationCommand(invitationEventId,invitationGuestId);
        return Result<CreateInvitationCommand>.Ok(command);
    }
}