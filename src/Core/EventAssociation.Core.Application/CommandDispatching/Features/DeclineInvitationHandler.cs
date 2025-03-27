using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class DeclineInvitationHandler : ICommandHandler<DeclineInvitationCommand>
{
    private readonly IInvitatonRepository _repository;
    private readonly IEventRepository _repositoryEvent;
    private readonly IUnitOfWork uow;

    public DeclineInvitationHandler(IInvitatonRepository repository, IEventRepository repositoryEvent, IUnitOfWork uow)
    {
        _repository = repository;
        _repositoryEvent = repositoryEvent;
        this.uow = uow;
    }


    public async Task<Result<None>> HandleAsync(DeclineInvitationCommand command)
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

        var declineInvitation = invitation.Result.Unwrap()
            .DeclineInvitation(invitationEvent.Result.Unwrap().GetEventStatus());

        if (!declineInvitation.IsSuccess)
        {
            return Result<None>.Err(declineInvitation.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return declineInvitation;
    }
}