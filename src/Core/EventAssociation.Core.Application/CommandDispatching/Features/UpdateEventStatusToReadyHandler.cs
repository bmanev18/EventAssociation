using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class UpdateEventStatusToReadyHandler: ICommandHandler<UpdateEventStatusToReadyCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;


    public UpdateEventStatusToReadyHandler(IEventRepository repository, IUnitOfWork uow)
    {
        this.repository = repository;
        this.uow = uow;
    }
    
    public async Task<Result<None>> HandleAsync(UpdateEventStatusToReadyCommand command)
    {
        var _event = await repository.GetAsync(command.Id);
        if (!_event.IsSuccess)
        {
            return Result<None>.Err(_event.UnwrapErr().ToArray());
        }

        var changeEventStatusToReadyResult = _event.Unwrap().ChangeEventStatusToReady();

        if (!changeEventStatusToReadyResult.IsSuccess)
        {
        return Result<None>.Err(changeEventStatusToReadyResult.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}