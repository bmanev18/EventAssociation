using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssociation.Infrastructure.SqliteDmPersistence;

public class DmContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Guest> Guests => Set<Guest>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureGuest(modelBuilder.Entity<Guest>());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmContext).Assembly);
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