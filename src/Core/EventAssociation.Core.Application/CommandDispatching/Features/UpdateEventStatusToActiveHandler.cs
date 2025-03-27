using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class UpdateEventStatusToActiveHandler: ICommandHandler<UpdateEventStatusToActiveCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;


    public UpdateEventStatusToActiveHandler(IEventRepository repository, IUnitOfWork uow)
    {
        this.repository = repository;
        this.uow = uow;
    }
    
    public async Task<Result<None>> HandleAsync(UpdateEventStatusToActiveCommand command)
    {
        var _event = await repository.GetAsync(command.Id);
        if (!_event.IsSuccess)
        {
            return Result<None>.Err(_event.UnwrapErr().ToArray());
        }

        var changeEventStatusToActiveResult = _event.Unwrap().ChangeEventStatusToActive();

        if (!changeEventStatusToActiveResult.IsSuccess)
        {
        return Result<None>.Err(changeEventStatusToActiveResult.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}

