using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventMaxParticipants: ValueObject
{
    internal int Value { get;}
    private EventMaxParticipants(int value)
    {
        Value = value;
    }


    public static Result<EventMaxParticipants> Create(int value)
    {
        var validationResult = ValidateMaxParticipantsDoNotViolateTheLimit(value);
        var maxParticipants = new EventMaxParticipants(value);
        return Result<EventMaxParticipants>.Ok(maxParticipants);
    }


    private static Result<None> ValidateMaxParticipantsDoNotViolateTheLimit(int value)
    {
        
        switch (value)
        {
            case < 5:
                return Result<None>.Err(new Error("", "The max participants cannot be less than 5"));
            case > 50:
                return Result<None>.Err(new Error("", "The max participants cannot be greater than 50"));
        }   

        return Result<None>.Ok(None.Value);

    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}