using EventAssociation.Core.Application.CommandDispatching.Commands;

namespace UnitTests.Features.Invitation.DeclineInvitaton;

public class DeclineInvitationCommandTests
{
    [Fact]
    public void Create_WithValidGuid_ReturnsSuccessResult()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var invitationId = guid.ToString();
    
        // Act
        var result = DeclineInvitationCommand.Create(invitationId);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Unwrap());
        Assert.IsType<DeclineInvitationCommand>(result.Unwrap());
        Assert.Equal(result.Unwrap()._InvitationId.GetValue().ToString(), invitationId);
        
    }

    [Fact]
    public void Create_WithInvalidGuid_ThrowsFormatException()
    {
        // Arrange
        var invalidGuid = "this-is-not-a-guid";

        // Act & Assert
        Assert.Throws<FormatException>(() =>
        {
            DeclineInvitationCommand.Create(invalidGuid);
        });
    }
}