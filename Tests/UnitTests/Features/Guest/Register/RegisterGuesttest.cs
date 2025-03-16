
using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using UnitTests.Fakes;

namespace UnitTests.Features.Guest.Register;
using EventAssociation.Core.Domain.Aggregates.Guests;


public class RegisterGuesttest
{
    
    private readonly FakeEmailChecker fakeEmailChecker = new FakeEmailChecker();

    [Fact]
    public async Task Create_Guest_With_Unique_Email_Should_Succeed()
    {
        // Arrange
        var name = GuestName.Create("Michael", "Jackson").Unwrap();
        var email = GuestVIAEmail.Create("abc@via.dk").Unwrap();
        var image = GuestImageUrl.Create(new Uri("https://example.com/johndoe.jpg")).Unwrap();
        
        // Act
        var result = Guest.Create(name, email, image, fakeEmailChecker);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Unwrap());
        Assert.Equal(name, result.Unwrap().name);
        Assert.Equal(email, result.Unwrap().email);
        Assert.Equal(image, result.Unwrap().image);
    }

    [Fact]
    public async Task Create_Guest_With_Duplicate_Email_Should_Fail()
    {
        // Arrange
        var name = GuestName.Create("Michael", "Jackson").Unwrap();
        var email = GuestVIAEmail.Create("abcd@via.dk").Unwrap();
        var image = GuestImageUrl.Create(new Uri("https://example.com/johndoe.jpg")).Unwrap();
        
        // Act
        var result = Guest.Create(name, email, image, fakeEmailChecker);
    
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Email already exists", result.UnwrapErr().First().Message);
    }
    
    
}