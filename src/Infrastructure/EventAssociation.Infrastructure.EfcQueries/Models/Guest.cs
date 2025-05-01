namespace EventAssociation.Infrastructure.EfcQueries.Models;

public partial class Guest
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
