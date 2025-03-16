using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Guests;

public class Guest: AggregateRoot
{
    internal GuestId id;
    internal GuestName name;
    internal GuestVIAEmail email;
    internal GuestImageUrl image;


    private Guest(GuestId id, GuestName name, GuestVIAEmail email, GuestImageUrl image)
    {
        this.id = id;
        this.name = name;
        this.email = email;
        this.image = image;
    }

    public static Result<Guest> Create(GuestName name, GuestVIAEmail email, GuestImageUrl image, IEmailChecker emailChecker)
    {
        var guestId = new GuestId(Guid.NewGuid());
        var result = emailChecker.IsEmailUnique(email.Value);
        if (!result.Result)
        {
            return Result<Guest>.Err(new Error("", "Email already exists"));
        }

        var guest = new Guest(guestId, name, email, image);
        return Result<Guest>.Ok(guest);
    }
}