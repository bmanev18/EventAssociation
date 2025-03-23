using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class MakeEventPublicHandler : ICommandHandler<MakeEventPublicCommand>
{
    private readonly IEventRepository repository;
    private readonly IUnitOfWork uow;

    public async Task<Result<None>> HandleAsync(MakeEventPublicCommand command)
    {
        var getEvent = await repository.GetAsync(command.Id);
        if (!getEvent.IsSuccess)
        {
            return Result<None>.Err(getEvent.UnwrapErr().ToArray());
        }

        var changeEventTypeToPublic = getEvent.Unwrap().ChangeEventTypeToPublic();

        if (!changeEventTypeToPublic.IsSuccess)
        {
            return Result<None>.Err(changeEventTypeToPublic.UnwrapErr().ToArray());
        }

        await uow.SaveChangesAsync();
        return changeEventTypeToPublic;
    }
}