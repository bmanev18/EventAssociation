using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;


public class CreateInvitationTest
{
    [Fact]
    public void CreateInvitation_Success_WhenEventIsActiveAndGuestNotInvited()
    {
        // Arrange
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());
        var eventStatus = EventStatus.Active;
        bool isAlreadyInvited = false;
        bool isAlreadyParticipating = false;
        bool isEventFull = false;

        //Act
        var result = Invitation.Create(eventId, guestId, eventStatus, isAlreadyInvited, isAlreadyParticipating,
            isEventFull);

        // Assert
        Assert.True(result.IsSuccess);
        var invitation = result.Unwrap();
        Assert.NotNull(invitation);
        Assert.Equal(eventId, invitation.InvitationEventId);
        Assert.Equal(guestId, invitation.InvitationGuestId);
        Assert.Equal(InvitationStatus.Extended, invitation.InvitationStatus);
    }

    [Theory]
    [InlineData(EventStatus.Draft, "Guests can not be invited to events in Draft status")]
    [InlineData(EventStatus.Cancelled, "Guests can not be invited to events in Draft status")]
    public void CreateInvitation_Fails_WhenEventIsNotReadyOrActive(EventStatus eventStatus, string expectedErrorMessage)
    {
        // Arrange
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());
        bool isAlreadyInvited = false;
        bool isAlreadyParticipating = false;
        bool isEventFull = false;

        // Act
        var result = Invitation.Create(eventId, guestId, eventStatus, isAlreadyInvited, isAlreadyParticipating,
            isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == expectedErrorMessage);
    }

    [Fact]
    public void CreateInvitation_Fails_WhenGuestIsAlreadyInvited()
    {
        // Arrange
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());
        var eventStatus = EventStatus.Active;
        bool isAlreadyInvited = true;
        bool isAlreadyParticipating = false;
        bool isEventFull = false;

        // Act
        var result = Invitation.Create(eventId, guestId, eventStatus, isAlreadyInvited, isAlreadyParticipating,
            isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Guest is already invited.");
    }

    [Fact]
    public void CreateInvitation_Fails_WhenGuestIsAlreadyParticipating()
    {
        // Arrange
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());
        var eventStatus = EventStatus.Active;
        bool isAlreadyInvited = false;
        bool isAlreadyParticipating = true;
        bool isEventFull = false;

        // Act
        var result = Invitation.Create(eventId, guestId, eventStatus, isAlreadyInvited, isAlreadyParticipating,
            isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Guest is already participating.");
    }

    [Fact]
    public void CreateInvitation_Fails_WhenEventIsFull()
    {
        // Arrange
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());
        var eventStatus = EventStatus.Active;
        bool isAlreadyInvited = false;
        bool isAlreadyParticipating = false;
        bool isEventFull = true;

        // Act
        var result = Invitation.Create(eventId, guestId, eventStatus, isAlreadyInvited, isAlreadyParticipating,
            isEventFull);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.UnwrapErr(), e => e.Message == "Cannot invite guests, event is full.");
    }
}