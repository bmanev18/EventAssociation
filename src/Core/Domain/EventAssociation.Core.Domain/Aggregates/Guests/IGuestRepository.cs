using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests;

public interface IGuestRepository
{
    public Task<Result<None>> CreateAsync(Guest guest);
    public Task<Result<Guest>> GetAsync(GuestId guestId);
    public Task<Result<None>> RemoveAsync(GuestId guestId);
}