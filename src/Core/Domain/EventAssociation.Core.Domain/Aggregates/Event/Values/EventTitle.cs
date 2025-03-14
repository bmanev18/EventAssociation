using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Events.Values;

public class EventTitle: ValueObject
{
    public string Title { get; }
    
    public EventTitle(string title)
    {
        var responses = Validate(title);
        var assertion = Result<None>.AssertResponses(responses);
        
        if (assertion.IsSuccess) {
         Title = title;
        }
        
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }

    private List<Result<None>> Validate(string value)
    {
        var results = new List<Result<None>>();
        
        var result2 = CheckIsTitleInConstraints(value);
        results.Add(result2);
        return results;

    }
    private Result<None> CheckIsTitleInConstraints(string title)
    {
        switch (title.Length)
        {
            case 0:
                return Result<None>.Err(new Error(nameof(title), "Title can't be empty."));
            case < 3:
                return Result<None>.Err(new Error(nameof(title), "Title must be at least 3 characters."));
            case > 75: 
                return Result<None>.Err(new Error(nameof(title), "Title must be bellow 75 characters."));
            default:
                return Result<None>.Ok(None.Value);
            
        }
    }

}