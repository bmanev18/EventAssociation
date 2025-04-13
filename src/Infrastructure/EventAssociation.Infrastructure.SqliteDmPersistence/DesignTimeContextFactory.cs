using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventAssociation.Infrastructure.SqliteDmPersistence;

public class DesignTimeContextFactory: IDesignTimeDbContextFactory<DmContext>
{
    public DmContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DmContext>();
        optionsBuilder.UseSqlite(@"Data Source = EventAssociationProduction.db");
        return new DmContext(optionsBuilder.Options);
    }
    
}