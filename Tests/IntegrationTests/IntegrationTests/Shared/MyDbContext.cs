using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationTests.Shared;

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Guest> Guests { get; set; }

    public static MyDbContext SetupContext()
    {
        DbContextOptionsBuilder<MyDbContext> optionsBuilder = new();
        string testDatabaseName = Guid.NewGuid().ToString();
        optionsBuilder.UseSqlite(@"Data Source = " + testDatabaseName);
        MyDbContext context = new(optionsBuilder.Options);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
    
    public static async Task SaveAndClearAsync<T>(T entity, MyDbContext context) 
        where T : class
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
    
    public static async Task RemoveAndClearAsync<T>(T entity, MyDbContext context)
        where T : class
    {
        var key = context.Entry(entity).Property("id").CurrentValue;

        var trackedEntity = await context.Set<T>().FindAsync(key);
        if (trackedEntity is null)
            throw new InvalidOperationException("Entity not found for deletion.");

        context.Set<T>().Remove(trackedEntity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       ConfigureEvent(modelBuilder.Entity<Event>());
       ConfigureGuest(modelBuilder.Entity<Guest>());
    }

    private void ConfigureEvent(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.Id)
            .HasConversion(
                eId => eId.Value,
                dbValue => EventId.FromGuid(dbValue)
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