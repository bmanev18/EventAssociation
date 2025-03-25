using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class AcceptInvitationHandler : ICommandHandler<AcceptInvitationCommand>
{
    private readonly IInvitatonRepository _repository;
    private readonly IEventRepository _repositoryEvent;
    private readonly IUnitOfWork uow;
    
    public async Task<Result<None>> HandleAsync(AcceptInvitationCommand command)
    {
        var invitation = _repository.GetAsync(command._InvitationId);
        if (!invitation.Result.IsSuccess)
        {
            return Result<None>.Err(new Error("100", "Invitation does not exist."));
        }
        var invitationEvent = _repositoryEvent.GetAsync(invitation.Result.Unwrap().GetInvitationEventId());
        if (!invitationEvent.Result.IsSuccess)
        {
            return Result<None>.Err(new Error("100", "Event for invitation does not exist."));
        }
        
        //TODO: Change following checks to work when EFC figured out and guestlist has limit and to save to guest list
        //bool isEventFull = _invitationRepository.IsEventFull(command.InvitationEventId).Result.Unwrap();

        var acceptInvitation = invitation.Result.Unwrap()
            .AcceptInvitation(invitationEvent.Result.Unwrap().GetEventStatus(), false);

        if (!acceptInvitation.IsSuccess)
        {
            return Result<None>.Err(acceptInvitation.UnwrapErr().ToArray());
        }

        return acceptInvitation;
        
    }
}