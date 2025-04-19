using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestId: ValueObject
{
    public Guid Value { get; }
    
    private GuestId(Guid guid) => Value = guid;
    
    public static GuestId Create() => new GuestId(Guid.NewGuid());

    
    public static GuestId FromGuid(Guid guid) => new GuestId(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static explicit operator InvitationGuestId(GuestId guestId)
    {
        return new InvitationGuestId(guestId.Value);
    }
}