using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event;

public interface IEventRepository
{
    public Task<Result<Event>> CreateAsync(Event event_);
    public Task<Result<Event>> GetAsync(EventId eventId);
    public Task<Result<None>> RemoveAsync(EventId eventId);
}