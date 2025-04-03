using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Commands;

public class RegisterANewGuestCommand : ICommand
{
    internal GuestName guestName;
    internal GuestVIAEmail email;
    internal GuestImageUrl imageUrl;
    
    private RegisterANewGuestCommand(GuestName guestName, GuestVIAEmail email, GuestImageUrl imageUrl)
    {
        this.guestName = guestName;
        this.email = email;
        this.imageUrl = imageUrl;
    }

    public static Result<RegisterANewGuestCommand> Create(string firstName, string lastName, string email,
        string imageUrl)
    {
        var guestNameResult = GuestName.Create(firstName, lastName);
        var emailResult = GuestVIAEmail.Create(email);
        var imageUrlResult = GuestImageUrl.Create(new Uri(imageUrl));
        
        var errors = new List<Error>();
        
        if (!guestNameResult.IsSuccess) {
            errors.AddRange(guestNameResult.UnwrapErr());
        }
        
        if (!emailResult.IsSuccess) {
            errors.AddRange(emailResult.UnwrapErr());
        }
        
        if (!imageUrlResult.IsSuccess) {
            errors.AddRange(imageUrlResult.UnwrapErr());
        }

        if (errors.Any()) {
            return Result<RegisterANewGuestCommand>.Err(errors.ToArray());}
        
        
        var command = new RegisterANewGuestCommand(guestNameResult.Unwrap(), emailResult.Unwrap(), imageUrlResult.Unwrap());
        return Result<RegisterANewGuestCommand>.Ok(command);
    }
}