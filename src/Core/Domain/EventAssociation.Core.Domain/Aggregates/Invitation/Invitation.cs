using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Aggregates.Invitation;

public class Invitation: AggregateRoot
{
    internal  InvitationId InvitationId { get; private set; }
    
    internal InvitationEventId InvitationEventId { get; private set; }
    
    internal InvitationGuestId InvitationGuestId { get; private set; }
    
    internal InvitationStatus InvitationStatus { get; private set; }

    private Invitation(InvitationEventId invitationEventId, InvitationGuestId invitationGuestId, InvitationStatus invitationStatus)
    {
        InvitationId = new InvitationId(Guid.NewGuid());
        InvitationEventId = invitationEventId;
        InvitationGuestId = invitationGuestId;
        InvitationStatus = invitationStatus;
    }
    
    public static Result<Invitation> Create(
        InvitationEventId invitationEventId,
        InvitationGuestId invitationGuestId,
        EventStatus eventStatus, 
        bool isAlreadyInvited,
        bool isAlreadyParticipating,
        bool isEventFull)
    {
        var errors = new List<Error>();

        if (eventStatus == EventStatus.Draft)
        {
            errors.Add(new Error("100","Guests can not be invited to events in Draft status"));
        }

        if (eventStatus == EventStatus.Cancelled)
        {
            errors.Add(new Error("100","Guests can not be invited to events in Draft status"));
        }

        if (isAlreadyInvited)
        {
            errors.Add(new Error("100", "Guest is already invited."));
        }

        if (isAlreadyParticipating)
        {
            errors.Add(new Error("100", "Guest is already participating."));
        }

        if (isEventFull)
        {
            errors.Add(new Error("100", "Cannot invite guests, event is full."));
        }

        if (errors.Any())
        {
            return Result<Invitation>.Err(errors.ToArray());
        }

        return Result<Invitation>.Ok(new Invitation( invitationEventId, invitationGuestId, InvitationStatus.Extended));
    }
    
    public Result<None> AcceptInvitation(EventStatus eventStatus, bool isEventFull)
    {
        var errors = new List<Error>();
        
        if (eventStatus == EventStatus.Cancelled)
        {
            errors.Add(new Error("100", "Cancelled events cannot be joined."));
        }
        
        if (eventStatus == EventStatus.Ready)
        {
            errors.Add(new Error("100", "Cannot join event yet, event is not active."));
        }
        
        if (isEventFull)
        {
            errors.Add(new Error("100", "Cannot accept invitation, event is full."));
        }
        
        if (errors.Any())
        {
            return Result<None>.Err(errors.ToArray());
        }
        
        InvitationStatus = InvitationStatus.Accepted;
        return Result<None>.Ok(None.Value);
    }

    public Result<None> DeclineInvitation(EventStatus eventStatus)
    {
        var errors = new List<Error>();

        if (eventStatus == EventStatus.Cancelled)
        {
            errors.Add(new Error("100", "Cancelled events cannot be declined."));
        }
        
        if (errors.Any())
        {
            return Result<None>.Err(errors.ToArray());
        }
        
        InvitationStatus = InvitationStatus.Rejected;
        return Result<None>.Ok(None.Value);
    }
    
}