using EventAssociation.Core.Domain.Aggregates.Events.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;

public class AcceptInvitationTests
{
    [Fact]
    public void AcceptInvitation_Successful_WhenEventIsActive_AndNotFull()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Active;
        var isEventFull = false;

        // Act
        var result = invitation.AcceptInvitation( eventStatus, isEventFull);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(InvitationStatus.Accepted, invitation.InvitationStatus);
    }
    
    [Fact]
    public void AcceptInvitation_Fails_WhenEventIsCancelled()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Cancelled;
        var isEventFull = false;

        // Act
        var result = invitation.AcceptInvitation(eventStatus, isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Cancelled events cannot be joined.");
    }

    [Fact]
    public void AcceptInvitation_Fails_WhenEventIsFull()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Active;
        var isEventFull = true;

        // Act
        var result = invitation.AcceptInvitation(eventStatus, isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Cannot accept invitation, event is full.");
    }

    [Fact]
    public void AcceptInvitation_Fails_WhenEventIsReady()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Ready;
        var isEventFull = false;

        // Act
        var result = invitation.AcceptInvitation(eventStatus, isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Cannot join event yet, event is not active.");
    }
    
    [Fact]
    public void AcceptInvitation_Fails_WithMultipleErrors()
    {
        // Arrange
        var invitation = CreateTestInvitation();
        var eventStatus = EventStatus.Cancelled; 
        var isEventFull = true;                 

        // Act
        var result = invitation.AcceptInvitation(eventStatus, isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        var errors = result.UnwrapErr();
        
        Assert.Contains(errors, e => e.Message == "Cancelled events cannot be joined.");
        Assert.Contains(errors, e => e.Message == "Cannot accept invitation, event is full.");
        Assert.Equal(2, errors.Count); 
    }
    
    private Invitation CreateTestInvitation()
    {
        var invitationEventId = new InvitationEventId(Guid.NewGuid());
        var invitationGuestId = new InvitationGuestId(Guid.NewGuid());
        return Invitation.Create(invitationEventId, invitationGuestId, EventStatus.Active, false, false, false).Unwrap();
    }
}