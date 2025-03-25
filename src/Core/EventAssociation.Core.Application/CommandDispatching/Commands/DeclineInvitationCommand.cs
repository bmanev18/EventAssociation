using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class DeclineInvitationCommand
{
    internal InvitationId _InvitationId { get; }

    private DeclineInvitationCommand(InvitationId invitationId)
    {
        _InvitationId = invitationId;
    }
    
    public static Result<DeclineInvitationCommand> Create(string invitationId)
    {
        InvitationId result = new InvitationId(Guid.Parse(invitationId));
        return Result<DeclineInvitationCommand>.Ok(new DeclineInvitationCommand(result));
    }
}