using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssociation.Infrastructure.SqliteDmPersistence;

public class DmContext : DbContext
{
    
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Location> Locations => Set<Location>();
    
    public DbSet<Event> Events => Set<Event>();
    
    public DmContext(DbContextOptions<DmContext> options) : base(options)
    {
    }
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
        
        builder
            .HasOne<Location>("Location")
            .WithMany()
            .HasForeignKey("LocationId")
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property<LocationId>("LocationId")
            .HasConversion(
                id => id.Value,
                value => LocationId.FromGuid(value)
            );

        builder
            .Property(e => e.Title)
            .HasConversion(
                titleVo =>  titleVo.Title,
                title => EventTitle.CreateEventTitle(title).Unwrap()
                );
        
        builder
            .Property(e => e.Description)
            .HasConversion(
                descriptionVo =>  descriptionVo.Description,
                description => EventDescription.CreateEventDescription(description).Unwrap()
            );
        
        builder
            .Property(e => e.StartDate)
            .HasConversion(
                startDateVo =>  startDateVo.Value,
                startDate => EventTime.Create(startDate.ToString()).Unwrap()
            );
        
        builder
            .Property(e => e.EndDate)
            .HasConversion(
                endDateVo =>  endDateVo.Value,
                endDate => EventTime.Create(endDate.ToString()).Unwrap()
            );
        
        builder
            .Property(e => e.MaxParticipants)
            .HasConversion(
                maxParticipantsVo =>  maxParticipantsVo.Value,
                maxParticipants => EventMaxParticipants.Create(maxParticipants).Unwrap()
            );

        builder.Property<EventType>("Type").HasConversion(
            Type => Type.ToString(),
            value =>(EventType)Enum.Parse(typeof(EventType), value));
        
        builder.Property<EventStatus>("Status").HasConversion(
            Status => Status.ToString(),
            value =>(EventStatus)Enum.Parse(typeof(EventType), value));
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