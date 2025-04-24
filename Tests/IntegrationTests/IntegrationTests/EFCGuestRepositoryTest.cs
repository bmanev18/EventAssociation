using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using IntegrationTests.Shared;
using Microsoft.EntityFrameworkCore;
using UnitTests.Fakes;
using Xunit;

namespace IntegrationTests;
public class EFCGuestRepositoryTest
{
    private readonly FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();

    private Guest CreateGuest()
    {
        var name = GuestName.Create("Michael", "Jackson").Unwrap();
        var email = GuestVIAEmail.Create("abc@via.dk").Unwrap();
        var image = GuestImageUrl.Create(new Uri("https://example.com/johndoe.jpg")).Unwrap();
        var guest = Guest.Create(name, email, image, fakeEmailChecker);
        return guest.Unwrap();
    }

    [Fact]
    public async Task CreateGuest_ShouldPersistToDatabase()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();

        await MyDbContext.SaveAndClearAsync(guest, context);

        var result = await context.Set<Guest>().ToListAsync();

        Assert.Single(result);
        Assert.Equal("abc@via.dk", result.First().email.Value);
    }

    
    // [Fact(Skip = "too much for now :(")]
    [Fact]
    public async Task RemoveGuest_ShouldPersistToDatabase()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();

        await MyDbContext.SaveAndClearAsync(guest, context);

        var resultBeforeRemove = await context.Set<Guest>().ToListAsync();
        Assert.Single(resultBeforeRemove);

        await MyDbContext.RemoveAndClearAsync(guest, context);

        var resultAfterRemove = await context.Set<Guest>().ToListAsync();
        Assert.Empty(resultAfterRemove);
    }

    [Fact]
    public async Task GetGuests_WhenNoneExist_ShouldReturnEmptyList()
    {
        var context = MyDbContext.SetupContext();

        var result = await context.Set<Guest>().ToListAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGuests_WhenExist_ShouldReturnList()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();
        await MyDbContext.SaveAndClearAsync(guest, context);

        var result = await context.Set<Guest>().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal("abc@via.dk", result.First().email.Value);
    }

    [Fact]
    public async Task EnsureGuestNameIsCorrect()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();
        await MyDbContext.SaveAndClearAsync(guest, context);

        var result = await context.Set<Guest>().ToListAsync();

        Assert.Single(result);
        Assert.Equal("Michael", result.First().name.firstName);
    }

    [Fact]
    public async Task EnsureGuestImageUrlIsCorrect()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();
        await MyDbContext.SaveAndClearAsync(guest, context);

        var result = await context.Set<Guest>().ToListAsync();

        Assert.Single(result);
        Assert.Equal("https://example.com/johndoe.jpg", result.First().image.Value.ToString());
    }

    [Fact]
    public async Task EnsureGuestEmailIsCorrect()
    {
        var context = MyDbContext.SetupContext();
        var guest = CreateGuest();
        await MyDbContext.SaveAndClearAsync(guest, context);

        var result = await context.Set<Guest>().ToListAsync();

        Assert.Single(result);
        Assert.Equal("abc@via.dk", result.First().email.Value);
    }
}