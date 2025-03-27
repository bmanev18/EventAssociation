using EventAssociation.Core.Domain.Aggregates.Invitation;
using EventAssociation.Core.Tools.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAssociation.Core.Domain.Aggregates.Event.Values;

namespace UnitTests.Fakes
{
    public class FakeInvitationRepository : IInvitatonRepository
    {
        private readonly List<Invitation> _invitations;

        public FakeInvitationRepository()
        {
            _invitations = new List<Invitation>
            {
                CreateTestInvitation(),
                CreateTestInvitation(),
                CreateTestInvitation()
            };
        }

        private Invitation CreateTestInvitation()
        {
            var invitationEventId = new InvitationEventId(Guid.NewGuid());
            var invitationGuestId = new InvitationGuestId(Guid.NewGuid());
            return Invitation.Create(invitationEventId, invitationGuestId, EventStatus.Active, false, false, false).Unwrap();
        }

        public Task<Result<Invitation>> CreateAsync(Invitation invitation)
        {
            _invitations.Add(invitation);
            return Task.FromResult(Result<Invitation>.Ok(invitation));
        }

        public Task<Result<Invitation>> GetAsync(InvitationId invitationId)
        {
            var invitation = _invitations.FirstOrDefault(i => i.InvitationId.Equals(invitationId));
            return invitation != null
                ? Task.FromResult(Result<Invitation>.Ok(invitation))
                : Task.FromResult(Result<Invitation>.Err(new Error("", "Invitation not found")));
        }

        public Task<Result<None>> RemoveAsync(InvitationId invitationId)
        {
            var invitation = _invitations.FirstOrDefault(i => i.InvitationId.Equals(invitationId));
            if (invitation != null)
            {
                _invitations.Remove(invitation);
                return Task.FromResult(Result<None>.Ok(None.Value));
            }
            return Task.FromResult(Result<None>.Err(new Error("", "Invitation not found")));
        }

        public Task<Result<bool>> IsGuestAlreadyInvited(InvitationEventId eventId, InvitationGuestId guestId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> IsGuestParticipating(InvitationEventId eventId, InvitationGuestId guestId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> IsEventFull(InvitationEventId eventId)
        {
            throw new NotImplementedException();
        }
    }
}
