using System.Text.Json;
using System.IO;
using EventAssociation.Infrastructure.EfcQueries.Models;

namespace EventAssociation.Infrastructure.EfcQueries.SeedFactories;

public class GuestSeedFactory
{
    private static string GuestsAsJson => File.ReadAllText(Path.Combine("Tests", "Mocks", "Guests.json"));

    public static List<Guest> CreateGuests()
    {
        
        var guestsTmps = JsonSerializer.Deserialize<List<TmpGuest>>(GuestsAsJson)!;

        var guests = guestsTmps.Select(g => new Guest
        {
            Id = g.Id,
            FirstName = g.FirstName,
            LastName = g.LastName,
            Email = g.Email,
            Image = g.Url
        }).ToList();

        return guests;
    }

    public record TmpGuest(string Id, string FirstName, string LastName, string Email, string Url);


}