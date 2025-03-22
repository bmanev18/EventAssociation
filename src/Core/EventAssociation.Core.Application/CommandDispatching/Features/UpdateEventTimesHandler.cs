using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class UpdateEventTimesHandler : ICommandHandler<UpdateEventTimesCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;

    internal UpdateEventTimesHandler(IEventRepository repository, IUnitOfWork uow) =>
        (this.repository, this.uow) = (repository, uow);


    public async Task<Result<None>> HandleAsync(UpdateEventTimesCommand command)
    {
        var getEvent = await repository.GetAsync(command.Id);

        if (!getEvent.IsSuccess)
        {
            return Result<None>.Err(getEvent.UnwrapErr().ToArray());
        }

        // TODO 
        var changedTimes = getEvent.Unwrap().ChangeTimes(command.StartDate, command.EndDate);

        if (!changedTimes.IsSuccess)
        {
            return Result<None>.Err(changedTimes.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}