using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Tools.OperationResult;
using EventAssociation.Infrastructure.SqliteDmPersistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqliteDmPersistence.Repositories;

public class GuestRepository : RepositoryBase<Guest, GuestId>, IGuestRepository {
    
    private readonly DbContext _context;
    
    public GuestRepository(DmContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Guest>>> GetAllAsync()
    {
        try
        {
            var guests = await _context.Set<Guest>().ToListAsync();
            return Result<List<Guest>>.Ok(guests);
        }
        catch (Exception ex)
        {
            return Result<List<Guest>>.Err(new Error("0",$"Error creating guest: {ex.Message}"));

        }
    }
    
    public async Task<Result<None>> CreateAsync(Guest guest)
    {
        try
        {
            await _context.Set<Guest>().AddAsync(guest);
            return Result<None>.Ok(None.Value);
        }
        catch (Exception ex)
        {
            return Result<None>.Err(new Error("0",$"Error creating guest: {ex.Message}"));
        }
    }
    
    public async Task<Result<Guest>> GetGuestAsync(Guest guestId)
    {
        var guest = await _context.Set<Guest>().FindAsync(guestId);
        if (guest == null) {
            return Result<Guest>.Err(new Error("0", "No event was found"));
        }
        return Result<Guest>.Ok(guest);
    }

    public async Task<Result<None>> RemoveAsync(Guest guestId)
    {
        var removedGuest = await _context.Set<Guest>().FindAsync(guestId);
        if (removedGuest == null) {
            return Result<None>.Err(new Error("0", "No event was found"));
        }
        _context.Set<Guest>().Remove(removedGuest);
        return Result<None>.Ok(None.Value);
    }
}
