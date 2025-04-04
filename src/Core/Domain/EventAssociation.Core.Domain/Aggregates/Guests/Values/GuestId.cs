using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestId: ValueObject
{
    private Guid Value { get; }
    
    public GuestId(Guid value)
    {
        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static explicit operator InvitationGuestId(GuestId guestId)
    {
        return new InvitationGuestId(guestId.Value);
    }
}