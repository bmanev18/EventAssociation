using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event.Values;

public class EventId: ValueObject
{
    public Guid Value { get; }

    private EventId()
    {
        
    }
    
    public EventId(Guid value)
    {
        Value = value;
    }
    
    public static EventId FromGuid(Guid guid) => new EventId(guid);

    public static Result<EventId> Create(string id)
    {
        try
        {
            var validId = Guid.Parse(id);
            return Result<EventId>.Ok(new EventId(validId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<EventId>.Err(new Error("Parse", e.Message));
        }
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator InvitationEventId(EventId eventId)
    {
        return new InvitationEventId(eventId.Value);
    }

}