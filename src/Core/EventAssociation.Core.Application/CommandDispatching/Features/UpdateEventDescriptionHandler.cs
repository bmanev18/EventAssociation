using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class UpdateEventDescriptionHandler : ICommandHandler<UpdateEventDescriptionCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;

    
    internal UpdateEventDescriptionHandler(IEventRepository repository, IUnitOfWork uow) =>
        (this.repository, this.uow) = (repository, uow);
    
    public async Task<Result<None>> HandleAsync(UpdateEventDescriptionCommand command)
    {
        var getEvent = await repository.GetAsync(command.Id);

        if (!getEvent.IsSuccess)
        {
            return Result<None>.Err(getEvent.UnwrapErr().ToArray());
        }

        var changeResult = getEvent.Unwrap().ChangeDescription(command.Description);

        if (!changeResult.IsSuccess)
        {
            return Result<None>.Err(changeResult.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}