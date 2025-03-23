using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class MakeEventPrivateHandler : ICommandHandler<MakeEventPrivateCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;

    public async Task<Result<None>> HandleAsync(MakeEventPrivateCommand command)
    {
        var getEvent = await repository.GetAsync(command.Id);
        if (!getEvent.IsSuccess)
        {
            return Result<None>.Err(getEvent.UnwrapErr().ToArray());
        }

        var changeEventTypeToPrivate = getEvent.Unwrap().ChangeEventTypeToPrivate();

        if (!changeEventTypeToPrivate.IsSuccess)
        {
            return Result<None>.Err(changeEventTypeToPrivate.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return changeEventTypeToPrivate;
    }
}