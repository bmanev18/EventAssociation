using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Application.CommandDispatching.Features;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Aggregates.Locations;
using EventAssociation.Core.Domain.Aggregates.Locations.Values;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;
using Moq;

namespace UnitTests.Features.Invitation.DeclineInvitaton;

public class DeclineInvitationCommandHandlerTests
{
    private readonly Mock<IInvitatonRepository> _invitationRepositoryMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public DeclineInvitationCommandHandlerTests()
    {
        _invitationRepositoryMock = new Mock<IInvitatonRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenInvitationDoesNotExist()
    {
        // Arrange
        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();
        _invitationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<InvitationId>()))
            .ReturnsAsync(
                Result<EventAssociation.Core.Domain.Aggregates.Invitation.Invitation>.Err(new Error("100",
                    "Invitation does not exist.")));

        // Act
        ICommandHandler<DeclineInvitationCommand> _handler =
            new DeclineInvitationHandler(_invitationRepositoryMock.Object, _eventRepositoryMock.Object,
                _unitOfWorkMock.Object);
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("100", result.UnwrapErr()[0].Code);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenEventDoesNotExist()
    {
        // Arrange
        var invitationId = new InvitationId(Guid.NewGuid());
        var invitation = EventAssociation.Core.Domain.Aggregates.Invitation.Invitation.Create(
            new InvitationEventId(Guid.NewGuid()),
            new InvitationGuestId(Guid.NewGuid()),
            EventStatus.Active,
            isAlreadyInvited: false,
            isAlreadyParticipating: false,
            isEventFull: false
        ).Unwrap();
        _invitationRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<InvitationId>()))
            .ReturnsAsync(Result<EventAssociation.Core.Domain.Aggregates.Invitation.Invitation>.Ok(invitation));
        _eventRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<EventId>()))
            .ReturnsAsync(
                Result<EventAssociation.Core.Domain.Aggregates.Event.Event>.Err(new Error("100",
                    "Event for invitation does not exist.")));

        var command = DeclineInvitationCommand.Create(Guid.NewGuid().ToString()).Unwrap();

        // Act
        ICommandHandler<DeclineInvitationCommand> _handler =
            new DeclineInvitationHandler(_invitationRepositoryMock.Object, _eventRepositoryMock.Object,
                _unitOfWorkMock.Object);
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Event for invitation does not exist.", result.UnwrapErr()[0].Message);
    }

    [Fact]
    public async Task HandleAsync_ShouldDeclineInvitation_WhenValid()
    {
        // Arrange
        var invitationId = Guid.NewGuid();
        var eventId = new InvitationEventId(Guid.NewGuid());
        var guestId = new InvitationGuestId(Guid.NewGuid());

        var invitation = EventAssociation.Core.Domain.Aggregates.Invitation.Invitation
            .Create(eventId, guestId, EventStatus.Active, false, false, false).Unwrap();


        var location1 = Location.CreateLocation(
            LocationType.Outside,
            LocationName.Create("Conference Hall").Unwrap(),
            LocationCapacity.Create(500).Unwrap()
        ).Unwrap();

        var eventStartDate1 = new EventTime(DateTime.UtcNow.AddDays(1));
        var eventEndDate1 = new EventTime(DateTime.UtcNow.AddDays(2));

        var eventResult1 = EventAssociation.Core.Domain.Aggregates.Event.Event
            .CreateEvent(location1, EventType.Public, eventStartDate1, eventEndDate1).Unwrap();

        var declineResult = invitation.DeclineInvitation(eventResult1.Status);
        Assert.True(declineResult.IsSuccess);

        _invitationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<InvitationId>()))
            .ReturnsAsync(Result<EventAssociation.Core.Domain.Aggregates.Invitation.Invitation>.Ok(invitation));
        _eventRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<EventId>()))
            .ReturnsAsync(Result<EventAssociation.Core.Domain.Aggregates.Event.Event>.Ok(eventResult1));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(Result<None>.Ok(None.Value));

        var command = DeclineInvitationCommand.Create(invitationId.ToString()).Unwrap(); // Convert Guid to string

        // Act
        ICommandHandler<DeclineInvitationCommand> _handler = new DeclineInvitationHandler(
            _invitationRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _unitOfWorkMock.Object
        );

        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}