using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestName: ValueObject
{
    public string firstName { get; private set; }
    public string lastName { get; private set; }



    private GuestName(string firstName, string lastName)
    {
        
        this.firstName = char.ToUpper(firstName[0]) + firstName.Substring(1);
        this.lastName = char.ToUpper(lastName[0]) + lastName.Substring(1);
        
    }

    public static Result<GuestName> Create(string firstName, string lastName)
    {
        var responses = Validate(firstName, lastName);
        
        var errors =  responses
            .Where(r => !r.IsSuccess)
            .SelectMany(r => r.UnwrapErr()) 
            .ToList();

        if (errors.Any())
        {
            return Result<GuestName>.Err(errors.ToArray());
        }
        
        var createdGuestName = new GuestName(firstName.ToLower(), lastName.ToLower());
        return Result<GuestName>.Ok(createdGuestName);
    }

    private static List<Result<None>> Validate(string firstName, string lastName)
    {
        return new List<Result<None>>
        {
            ValidateNotEmpty(firstName),
            ValidateNotEmpty(lastName),
            
            ValidateAboveMinimum(firstName),
            ValidateAboveMinimum(lastName),
            
            ValidateUnderMaximum(firstName),
            ValidateUnderMaximum(lastName),

            ValidateNoNumbers(firstName),
            ValidateNoNumbers(lastName)
        };
    }

    private static Result<None> ValidateNotEmpty(string anyname)
    {
        if (string.IsNullOrEmpty(anyname))
        {
            return Result<None>.Err(new Error(" ", "Cannot have empty names!"));
        }
        return Result<None>.Ok(None.Value);
    }

    private static Result<None> ValidateNoNumbers(string anyname)
    {
        if (anyname.Any(c => !char.IsLetter(c)))
        {
            return Result<None>.Err(new Error(" ", $"{anyname} contains invalid characters. Only letters are allowed."));
        }
        return Result<None>.Ok(None.Value);
    }
    
    private static Result<None> ValidateAboveMinimum(string anyname)
    {
        if (anyname.Length < 2)
        {
            return Result<None>.Err(new Error(" ", $"{anyname} is too short, use at least 2 characters."));
        }
        return Result<None>.Ok(None.Value);
    }
    
    private static Result<None> ValidateUnderMaximum(string anyname)
    {
        if (anyname.Length > 25)
        {
            return Result<None>.Err(new Error(" ", $"{anyname} is too long, keep it under 2 characters."));
        }
        return Result<None>.Ok(None.Value);
        
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
    private GuestName()
    {
        
    }
}