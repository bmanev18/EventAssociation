using EventAssociation.Core.Domain.Aggregates.Event;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Shared;

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    
    public DbSet<Event> Events { get; set; }
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
    
    private static async Task SaveAndClearAsync<T>(T entity, MyDbContext context) 
        where T : class
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
    
}