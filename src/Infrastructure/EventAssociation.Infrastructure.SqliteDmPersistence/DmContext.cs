using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqliteDmPersistence;

public class DmContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmContext).Assembly);
    }
    
}