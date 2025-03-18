using EventAssociation.Core.Domain.Common.Values;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public class InvitationId: ValueObject
{
    private Guid Value { get; }
    
    public InvitationId(Guid value)
    {
        Value = value;
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}