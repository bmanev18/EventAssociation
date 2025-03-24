using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class AcceptInvitationHandler : ICommandHandler<AcceptInvitationCommand>
{
    private readonly IInvitatonRepository _repository;
    private readonly IEventRepository _repositoryEvent;
    private readonly IUnitOfWork uow;
    
    public Task<Result<None>> HandleAsync(AcceptInvitationCommand command)
    {
        //use event repository by getting event status and isfull from eventid taken from
        // Inivitation taken from invitation repository from command.InvitationId
     
        throw new NotImplementedException();
    }
}