using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeGuestRepository : IGuestRepository
{
    private List<Guest> _guests;

    public FakeGuestRepository(IEmailChecker emailChecker)
    {
        _guests =
        [
            Guest.Create(GuestName.Create("John", "Doe").Unwrap(), GuestVIAEmail.Create("000000@via.dk").Unwrap(),
                GuestImageUrl.Create(new Uri("https://example.com/john.jpg")).Unwrap(), emailChecker).Unwrap(),
            Guest.Create(GuestName.Create("Jack", "Smith").Unwrap(), GuestVIAEmail.Create("000001@via.dk").Unwrap(),
                GuestImageUrl.Create(new Uri("https://example.com/john.jpg")).Unwrap(), emailChecker).Unwrap(),
            Guest.Create(GuestName.Create("Kate", "Bush").Unwrap(), GuestVIAEmail.Create("000002@via.dk").Unwrap(),
                GuestImageUrl.Create(new Uri("https://example.com/john.jpg")).Unwrap(), emailChecker).Unwrap(),
            Guest.Create(GuestName.Create("Peter", "Griffin").Unwrap(), GuestVIAEmail.Create("000003@via.dk").Unwrap(),
                GuestImageUrl.Create(new Uri("https://example.com/john.jpg")).Unwrap(), emailChecker).Unwrap(),
            Guest.Create(GuestName.Create("da", "baby").Unwrap(), GuestVIAEmail.Create("000004@via.dk").Unwrap(),
                GuestImageUrl.Create(new Uri("https://example.com/john.jpg")).Unwrap(), emailChecker).Unwrap()

        ];        
    }
    
    public Task<Result<None>> CreateAsync(Guest guest)
    {
        _guests.Add(guest);
        return Task.FromResult(Result<None>.Ok(None.Value));
    }

    public Task<Result<Guest>> GetAsync(GuestId guestId)
    {
        var guest = _guests.FirstOrDefault(g => g.id.Equals(guestId));
        return guest != null 
            ? Task.FromResult(Result<Guest>.Ok(guest)) 
            : Task.FromResult(Result<Guest>.Err(new Error("", "Guest not found")));
    }

    public Task<Result<None>> RemoveAsync(GuestId guestId)
    {
        var guest = _guests.FirstOrDefault(g => g.id.Equals(guestId));
        if (guest != null)
        {
            _guests.Remove(guest);
            return Task.FromResult(Result<None>.Ok(None.Value));
        }
        return Task.FromResult(Result<None>.Err(new Error("", "Guest not found")));
    }
}