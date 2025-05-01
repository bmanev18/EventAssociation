namespace EventAssociation.Infrastructure.EfcQueries.Models;

public partial class Location
{
    public string Id { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int LocationCapacity { get; set; }

    public string LocationName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
