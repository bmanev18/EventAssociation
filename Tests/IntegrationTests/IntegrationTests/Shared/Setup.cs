using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Infrastructure.EfcQueries.Models;
using EventAssociation.Infrastructure.EfcQueries.SeedFactories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationTests.Shared;

public class Setup(DbContextOptions options) :DbContext(options)
{
    public static EventAssociationProductionContext Seed(EventAssociationProductionContext context)
    {
        context.Events.AddRange(EventSeedFactory.CreateEvents());
        context.SaveChanges();
        return context;
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<Guest> Guests { get; set; }

    public static EventAssociationProductionContext SetupContext()
    {
        DbContextOptionsBuilder<EventAssociationProductionContext> optionsBuilder = new();
        string testDatabaseName = Guid.NewGuid().ToString();
        optionsBuilder.UseSqlite(@"Data Source = " + testDatabaseName);
        EventAssociationProductionContext context = new(optionsBuilder.Options);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ConfigureEvent(modelBuilder.Entity<Event>());
        // ConfigureGuest(modelBuilder.Entity<Guest>());
    }
    
}