using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;


public class DeclineInvitationTests
{
    [Fact]
    public void DeclineInvitation_Success_WhenPending()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Active;

        // Act
        var result = invitation.DeclineInvitation(eventStatus);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(InvitationStatus.Rejected, invitation.InvitationStatus);
    }

    [Fact]
    public void DeclineInvitation_Success_WhenAccepted()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Active;

        // Act
        invitation.AcceptInvitation(EventStatus.Active, false); //Event accepted
        var result = invitation.DeclineInvitation(eventStatus);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(InvitationStatus.Rejected, invitation.InvitationStatus);
    }

    [Fact]
    public void DeclineInvitation_Fails_IfEventCancelled()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Cancelled;

        // Act
        var result = invitation.DeclineInvitation(eventStatus);

        // Assert
        Assert.False(result.IsSuccess);
        var errors = result.UnwrapErr();
        Assert.Contains(errors, e => e.Message == "Cancelled events cannot be declined.");
    }

    private Invitation CreateTestInvitation()
    {
        var invitationEventId = new InvitationEventId(Guid.NewGuid());
        var invitationGuestId = new InvitationGuestId(Guid.NewGuid());
        return Invitation.Create(invitationEventId, invitationGuestId, EventStatus.Active, false, false, false).Unwrap();
    }
}