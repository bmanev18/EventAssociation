using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestImageUrl : ValueObject
{
    internal Uri Value { get; }

    private GuestImageUrl(Uri value)
    {
        Value = value;
    }

    public static Result<GuestImageUrl> Create(Uri url)
    {
        var result = new GuestImageUrl(url);
        return Result<GuestImageUrl>.Ok(result);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}