﻿using System.Text.Json;
using System.IO;
using System.Globalization;
using EventAssociation.Infrastructure.EfcQueries.Models;

namespace EventAssociation.Infrastructure.EfcQueries.SeedFactories;

public class LocationSeedFactory
{
    private static string LocationsAsJson => File.ReadAllText(Path.Combine("Tests", "Mocks", "Locations.json"));

    public static List<Location> CreateLocations()
    {
        List<TmpLocation> locationsTmps = JsonSerializer.Deserialize<List<TmpLocation>>(LocationsAsJson)!;

        var locations = locationsTmps.Select(l => new Location
        {
            Id = l.Id,
            LocationName = l.Name,
            LocationCapacity = l.MaxCapacity,
            // Assuming Status field could be derived from availability dates
            // If current date is within availability range, set status to "Available", otherwise "Unavailable"
            Status = IsDateInRange(l.AvailabilityStart, l.AvailabilityEnd) ? "Available" : "Unavailable"
        }).ToList();

        return locations;
    }

    private static bool IsDateInRange(string startDateStr, string endDateStr)
    {
        // Parse dates in the format "dd-MM-yyyy"
        if (DateTime.TryParseExact(startDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate) &&
            DateTime.TryParseExact(endDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
        {
            DateTime currentDate = DateTime.Today;
            return currentDate >= startDate && currentDate <= endDate;
        }
        
        return false;
    }

    public record TmpLocation(string Id, string Name, int MaxCapacity, string AvailabilityStart, string AvailabilityEnd);
}