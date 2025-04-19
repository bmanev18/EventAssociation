using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssociation.Infrastructure.SqliteDmPersistence;

public class DmContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Location> Locations => Set<Location>();
    
    public DbSet<Event> Events => Set<Event>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureGuest(modelBuilder.Entity<Guest>());
        ConfigureLocation(modelBuilder.Entity<Location>());
        ConfigureEvent(modelBuilder.Entity<Event>());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmContext).Assembly);
    }

    private void ConfigureEvent(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .Property(m => m.Id)
            .HasConversion(
                mId => mId.Value,
                dbValue => EventId.FromGuid(dbValue)
            );
    }

    private void ConfigureLocation(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder
            .Property(m => m.Id)
            .HasConversion(
                mId => mId.Value,
                dbValue => LocationId.FromGuid(dbValue)
            );
        
        builder.Property<LocationType>("status")
            .HasConversion(
                status => status.ToString(), 
                value => (LocationType)Enum.Parse(typeof(LocationType), value)
            );
        
        builder.ComplexProperty<LocationName>(
            "LocationName",
            propBuilder =>
            {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("LocationName");
            }
        );
        
        builder.ComplexProperty<LocationCapacity>(
            "LocationCapacity",
            propBuilder =>
            {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("LocationCapacity");
            }
        );
    }

    private void ConfigureGuest(EntityTypeBuilder<Guest> builder)
    {
        builder.HasKey(x => x.id);

        builder
            .Property(m => m.id)
            .HasConversion(
                mId => mId.Value,
                dbValue => GuestId.FromGuid(dbValue)
            );
        
        builder.ComplexProperty<GuestName>("name", propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.firstName)
                .HasColumnName("firstName");

            propBuilder.Property(valueObjectP => valueObjectP.lastName)
                .HasColumnName("lastName");
        });
        
        
        builder.ComplexProperty<GuestVIAEmail>(
            "email",
            propBuilder =>
            {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("email");
            }
        );

        builder.ComplexProperty<GuestImageUrl>(
            "image",
            propBuilder =>
            {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("image")
                    .HasConversion(
                        uri => uri.ToString(), 
                        str => new Uri(str) 
                    );
            }
        );

    }
    
}