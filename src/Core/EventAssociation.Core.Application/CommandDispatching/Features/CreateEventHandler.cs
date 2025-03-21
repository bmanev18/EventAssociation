using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class CreateEventHandler: ICommandHandler<CreateEventCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;

    internal CreateEventHandler(IEventRepository repository, IUnitOfWork uow) =>
        (this.repository, this.uow) = (repository, uow); 
    
    public async Task<Result<None>> HandleAsync(CreateEventCommand command)
    {
        var event_ = Event.CreateEvent(command.Location_, command.Type, command.StartDate, command.EndDate);
        if (!event_.IsSuccess)
        {
            return Result<None>.Err(new Error("Handler", "Event could not be created"));
        }

        var a = await repository.CreateAsync(event_.Unwrap());
        if (!a.IsSuccess)
        {
            return Result<None>.Err(new Error("Repository", "Event not saved in Repository"));
 
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}