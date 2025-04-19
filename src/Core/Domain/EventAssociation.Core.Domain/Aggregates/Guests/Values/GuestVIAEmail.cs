using System.Text.RegularExpressions;
using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestVIAEmail: ValueObject
{
    public string Value { get; }

    private GuestVIAEmail()
    {
        
    }
    
    private GuestVIAEmail(string value)
    {
        Value = value;
    }

    public static Result<GuestVIAEmail> Create(string email)
    {
        var results = Validate(email);
        var errors =  results
            .Where(r => !r.IsSuccess)
            .SelectMany(r => r.UnwrapErr()) 
            .ToList();

        if (errors.Any())
        {
            return Result<GuestVIAEmail>.Err(errors.ToArray());
        }

        var VIAemail = new GuestVIAEmail(email.ToLower());
        return Result<GuestVIAEmail>.Ok(VIAemail);
    }

    private static List<Result<None>> Validate(string email)
    {
        var validationResults = new List<Result<None>>
        {
            ValidateNotEmpty(email),
            ValidateCorrectFormat(email),
            ValidateEndsWithViaDk(email),
        };
        return validationResults;
    }
    private static Result<None> ValidateNotEmpty(string email)
    {
        return string.IsNullOrWhiteSpace(email)
            ? Result<None>.Err(new Error(nameof(email), "Email is required"))
            : Result<None>.Ok(None.Value);
    }
    private static Result<None> ValidateCorrectFormat(string email)
    {
        var pattern = @"^(?:(?:[a-zA-Z]{3,4})|\d{6})@via\.dk$"; //should avoid regex, but I have the lazies
        return !Regex.IsMatch(email, pattern)
            ? Result<None>.Err(new Error(nameof(email), "Incorrect email format"))
            : Result<None>.Ok(None.Value);
    }

    private static Result<None> ValidateEndsWithViaDk(string email)
    {
        return !email.EndsWith("@via.dk")
            ? Result<None>.Err(new Error(nameof(email), "Email must end with '@via.dk'"))
            : Result<None>.Ok(None.Value);
    }
    

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}