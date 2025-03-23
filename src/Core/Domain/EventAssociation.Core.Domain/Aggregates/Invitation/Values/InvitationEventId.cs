using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public class InvitationEventId : ValueObject
{
    private Guid Value { get; }
    
    public InvitationEventId(Guid value)
    {
        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator EventId(InvitationEventId invitationEventId)
    {
        return new EventId(invitationEventId.Value);
    }

}