using System.Windows.Input;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class RegisterANewGuestHandler: ICommandHandler<RegisterANewGuestCommand>
{
    private readonly IGuestRepository repository;
    private readonly IUnitOfWork uow;
    private IEmailChecker emailChecker;

    public RegisterANewGuestHandler(IGuestRepository repository, IUnitOfWork uow, IEmailChecker emailChecker) {
        this.repository = repository;
        this.uow = uow;
        this.emailChecker = emailChecker;
        
    }
    
    public async Task<Result<None>> HandleAsync(RegisterANewGuestCommand command) {

        var guest = Guest.Create(command.guestName, command.email, command.imageUrl, emailChecker);
        
        if (!guest.IsSuccess) {
            return Result<None>.Err(guest.UnwrapErr().ToArray());
        }
        
        var newGuest  = await repository.CreateAsync(guest.Unwrap());
        if (!newGuest.IsSuccess) {
            return Result<None>.Err(new Error("Repository", "Guest not saved in Repository"));
        }
        
        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}