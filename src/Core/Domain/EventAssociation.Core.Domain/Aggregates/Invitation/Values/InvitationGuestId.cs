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
}