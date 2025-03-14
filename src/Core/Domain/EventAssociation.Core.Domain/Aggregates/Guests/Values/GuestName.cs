using EventAssociation.Core.Domain.Common.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class GuestName: ValueObject
{
    private string firstName { get; }
    private string lastName { get; }


    private GuestName(string firstName, string lastName)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        
    }

    public static Result<GuestName> Create(string firstName, string lastName)
    {
        var createdGuestName = new GuestName(firstName, lastName);
        return Result<GuestName>.Ok(createdGuestName);
    }

    private static List<Result<None>> Validate(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}