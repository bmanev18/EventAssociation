using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class AcceptInvitationCommand
{
    internal InvitationId _InvitationId { get; }

    private AcceptInvitationCommand(InvitationId invitationId)
    {
        _InvitationId = invitationId;
    }

    public static Result<AcceptInvitationCommand> Create(string invitationId)
    {
            InvitationId result = new InvitationId(Guid.Parse(invitationId));
            return Result<AcceptInvitationCommand>.Ok(new AcceptInvitationCommand(result));
    }
    
    
}