﻿using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Event.Values;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Values;
using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching.Features;

public class CreateInvitationHandler : ICommandHandler<CreateInvitationCommand>
{
    private readonly IInvitatonRepository _invitationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork uow;

    public CreateInvitationHandler(IInvitatonRepository invitationRepository, IEventRepository eventRepository, IGuestRepository guestRepository, IUnitOfWork uow)
    {
        _invitationRepository = invitationRepository;
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        this.uow = uow;
    }


    public async Task<Result<None>> HandleAsync(CreateInvitationCommand command)
    {
        var eventExists = _eventRepository.GetAsync(command.InvitationEventId);//  _eventRepository.GetAsync(eventId);
        if (!eventExists.Result.IsSuccess)
        {
            return Result<None>.Err(new Error("100", "Event does not exist."));
        }
        
        var guestExists = _guestRepository.GetAsync((GuestId)command.InvitationGuestId);
        if (!guestExists.Result.IsSuccess)
        {
            return Result<None>.Err(new Error("100", "Guest does not exist."));
        }

        //TODO: Change following checks to work when EFC figured out and guestlist has limit
        //bool isAlreadyInvited = _invitationRepository.IsGuestAlreadyInvited(command.InvitationEventId, command.InvitationGuestId).Result.Unwrap();
        bool isAlreadyParticipating = eventExists.Result.Unwrap().IsGuestInGuestlist(guestExists.Result.Unwrap());
        //bool isEventFull = _invitationRepository.IsEventFull(command.InvitationEventId).Result.Unwrap();
        
        EventStatus eventStatus = eventExists.Result.Unwrap().GetEventStatus(); 
        
        var invitation = Invitation.Create(
            command.InvitationEventId,
            command.InvitationGuestId,
            eventStatus,
            false,
            isAlreadyParticipating,
            false
        );

        if (!invitation.IsSuccess)
        {
            return Result<None>.Err(new Error("Repository", "Invitation not saved in Repository"));
        }

        await uow.SaveChangesAsync();
        return Result<None>.Ok(None.Value);
    }
}