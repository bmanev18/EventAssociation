using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventDescription: ValueObject
{
    private string Value { get; }
    
    public EventDescription(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private List<Result<None>> Validate(string title)
    {
        var results = new List<Result<None>>();
        
        var result2 = CheckIsDescriptionInConstraints(title);
        results.Add(result2);
        return results;
    }

    private Result<None> CheckIsDescriptionInConstraints(string description)
    {
        switch (description.Length)
        {
            case > 250: 
                return Result<None>.Err(new Error(nameof(description), "Description must be bellow 250 characters."));
            default:
                return Result<None>.Ok(None.Value);
            
        }
    }
}