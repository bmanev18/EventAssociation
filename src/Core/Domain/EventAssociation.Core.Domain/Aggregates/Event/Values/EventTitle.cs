using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event.Values;

public class EventTitle : ValueObject
{
    internal string Title { get; }

    
    private EventTitle()
    {
    }
    private EventTitle(string title)
    {
        Title = title;
    }

    public static Result<EventTitle> CreateEventTitle(String title)
    {
        var responses = Validate(title);

        var errors = responses
            .Where(r => !r.IsSuccess)
            .SelectMany(r => r.UnwrapErr())
            .ToList();

        if (errors.Any())
        {
            return Result<EventTitle>.Err(errors.ToArray());
        }

        return Result<EventTitle>.Ok(new EventTitle(title));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
    }

    private static List<Result<None>> Validate(string value)
    {
        var results = new List<Result<None>>();

        var constraintsResult = CheckIsTitleInConstraints(value);
        results.Add(constraintsResult);
        return results;
    }

    private static Result<None> CheckIsTitleInConstraints(string title)
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

    public bool IsEmptyOrDefualt() => string.IsNullOrWhiteSpace(Title) || Title.Equals("Working Title");
}