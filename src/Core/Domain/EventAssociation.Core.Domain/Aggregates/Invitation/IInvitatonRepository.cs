using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public interface IInvitatonRepository
{
    public Task<Result<Invitation>> CreateAsync(Invitation invitation);
    public Task<Result<Invitation>> GetAsync(InvitationId invitationId);
    public Task<Result<None>> RemoveAsync(InvitationId guestId);
}