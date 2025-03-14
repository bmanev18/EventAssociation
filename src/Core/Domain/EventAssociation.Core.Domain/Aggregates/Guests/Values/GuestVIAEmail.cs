using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestVIAEmail: ValueObject
{
    internal string Value { get; }
    
    private GuestVIAEmail(string value)
    {
        Value = value;
    }

    public static Result<GuestVIAEmail> Create(string email)
    {
        var VIAemail = new GuestVIAEmail(email);
        return Result<GuestVIAEmail>.Ok(VIAemail);
    }

    public List<Result<None>> Validate()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}