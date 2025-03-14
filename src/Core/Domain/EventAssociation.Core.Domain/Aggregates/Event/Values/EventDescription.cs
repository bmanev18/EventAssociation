using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventDescription: ValueObject
{
    internal string Description { get; }
    
    private EventDescription(string description)//TODO-Change to private
    {
        Description = description;
    }

    public static Result<EventDescription> CreateEventDescription(string description)
    {
        var responses = Validate(description);
        
        var errors =  responses
            .Where(r => !r.IsSuccess)
            .SelectMany(r => r.UnwrapErr()) 
            .ToList();

        if (errors.Any())
        {
            return Result<EventDescription>.Err(errors.ToArray());
        }

        return Result<EventDescription>.Ok(new EventDescription(description));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }

    private static List<Result<None>> Validate(string title)
    {
        var results = new List<Result<None>>();
        
        var constraintsResult = CheckIsDescriptionInConstraints(title);
        results.Add(constraintsResult);
        return results;
    }

    private static Result<None> CheckIsDescriptionInConstraints(string description)
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