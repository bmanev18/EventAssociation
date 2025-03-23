using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public interface IInvitatonRepository
{
    public Task<Result<Invitation>> CreateAsync(Invitation invitation);
    public Task<Result<Invitation>> GetAsync(InvitationId invitationId);
    public Task<Result<None>> RemoveAsync(InvitationId guestId);
    //TODO:MAY NEED TO MOVE THE FOLLOWING BELOW
    Task<Result<bool>> IsGuestAlreadyInvited(InvitationEventId eventId, InvitationGuestId guestId);
    
    Task<Result<bool>> IsGuestParticipating(InvitationEventId eventId, InvitationGuestId guestId);
    
    Task<Result<bool>> IsEventFull(InvitationEventId eventId);
}