using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;

namespace EventAssociation.Core.Application;

public class EmailChecker : IEmailChecker
{
    public Task<bool> IsEmailUnique(string email)
    {
        return Task.FromResult(true);
    }
}