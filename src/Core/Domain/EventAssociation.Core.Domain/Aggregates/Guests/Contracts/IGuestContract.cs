namespace EventAssociation.Core.Domain.Aggregates.Guests.Contracts;

    public interface IEmailChecker
    {
        Task<bool> IsEmailUnique(string email);
    }
