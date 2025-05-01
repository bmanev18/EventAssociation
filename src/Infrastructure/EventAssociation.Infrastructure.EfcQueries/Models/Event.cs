namespace EventAssociation.Infrastructure.EfcQueries.Models;

public partial class Event
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public int MaxParticipants { get; set; }

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string LocationId { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
