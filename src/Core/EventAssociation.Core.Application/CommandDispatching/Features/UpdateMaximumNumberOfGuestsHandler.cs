using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class UpdateMaximumNumberOfGuestsHandler: ICommandHandler<UpdateMaximumNumberOfGuestsCommand>
{
    
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;


    public UpdateMaximumNumberOfGuestsHandler(IEventRepository repository, IUnitOfWork uow)
    {
        this.repository = repository;
        this.uow = uow;
    }
    public async Task<Result<None>> HandleAsync(UpdateMaximumNumberOfGuestsCommand command)
    {
        var _event = await repository.GetAsync(command.Id);
        if (!_event.IsSuccess)
        {
            return Result<None>.Err(_event.UnwrapErr().ToArray());
        }

        var updateMaxNumberOfGuestsResult = _event.Unwrap().UpdateMaxNumberOfParticipants(command.MaxParticipants);

        if (!updateMaxNumberOfGuestsResult.IsSuccess) {
            return Result<None>.Err(updateMaxNumberOfGuestsResult.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}