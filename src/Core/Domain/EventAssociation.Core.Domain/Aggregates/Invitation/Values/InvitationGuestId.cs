using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public class InvitationGuestId : ValueObject
{
    private Guid Value { get; }
    
    public InvitationGuestId(Guid value)
    {
        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static explicit operator GuestId(InvitationGuestId invitationGuestId)
    {
        return new GuestId(invitationGuestId.Value);
    }
}