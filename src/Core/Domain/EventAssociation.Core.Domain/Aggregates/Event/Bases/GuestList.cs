using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Event.Bases;

public class GuestList
{
    private readonly List<Guest> _guests;

    private GuestList()
    {
        _guests = new List<Guest>();    
    }

    public static Result<GuestList> Create()
    {
        var guestList = new GuestList();
        return Result<GuestList>.Ok(guestList);
    }

    public Result<Guest> GetGuest(GuestId guestId)
    {
        var guest = _guests.FirstOrDefault(g => g.id.Equals(guestId));
        if (guest == null)
        {
            return Result<Guest>.Err(new Error("100", "Guest not found."));
        }

        return Result<Guest>.Ok(guest);
    }
    
    public Result<Guest> RemoveGuest(GuestId guestId)
    {
        var guest = _guests.FirstOrDefault(g => g.id.Equals(guestId));
        if (guest == null)
        {
            return Result<Guest>.Err(new Error("100", "Guest not found."));
        }

        _guests.Remove(guest);
        return Result<Guest>.Ok(guest);
    }
    
    public Result<None> AddGuest(Guest guest)
    {
        if (_guests.Any(g => g.id.Equals(guest.id)))
        {
            return Result<None>.Err(new Error("100", "Guest already exists."));
        }

        _guests.Add(guest);
        return Result<None>.Ok(None.Value);
    }
    
    public bool IsGuestAlreadyInList(Guest guest)
    {
        return _guests.Any(g => g.id.Equals(guest.id));
    }
    
    public int GetTotalGuests()
    {
        return _guests.Count;
    }
}