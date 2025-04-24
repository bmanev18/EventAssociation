using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Infrastructure.SqliteDmPersistence.Shared;
using Microsoft.EntityFrameworkCore;
using EventId = EventAssociation.Core.Domain.Aggregates.Event.Values.EventId;

namespace EventAssociation.Infrastructure.SqliteDmPersistence.Repositories;

public class EventRepository: RepositoryBase<Event, EventId>, IEventRepository
{
    
    private readonly DbContext _context;
    
    public EventRepository(DmContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Result<None>> CreateAsync(Event event_)
    {
        try
        {
            await _context.Set<Event>().AddAsync(event_);
            return Result<None>.Ok(None.Value);
        }
        catch (Exception ex)
        {
            return Result<None>.Err(new Error("0",$"Error creating guest: {ex.Message}"));
        }
    }
    
    public async Task<Result<Event>> GetAsync(EventId eventId)
    {
        var event_ = await _context.Set<Event>().FindAsync(eventId);
        if (event_ == null) {
            return Result<Event>.Err(new Error("0", "No event was found"));
        }
        return Result<Event>.Ok(event_);
    }

    public async Task<Result<None>> RemoveAsync(EventId eventId)
    {
        var event_ = await _context.Set<Event>().FindAsync(eventId);
        if (event_ == null) {
            return Result<None>.Err(new Error("0", "No event was found"));
        }
        _context.Set<Event>().Remove(event_);
        return Result<None>.Ok(None.Value);
    }

    public async Task<Result<List<Event>>> GetAllEvents()
    {
        try {
            var guests = await _context.Set<Event>().ToListAsync();
            return Result<List<Event>>.Ok(guests);
        }
        catch (Exception ex) {
            return Result<List<Event>>.Err(new Error("0",$"Error creating guest: {ex.Message}"));
        }
    }
}

