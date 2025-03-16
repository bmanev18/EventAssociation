using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;

namespace UnitTests.Fakes;

public class FakeEmailChecker : IEmailChecker
{
    private readonly HashSet<string> emails;
        public FakeEmailChecker()
        {
            emails = new HashSet<string>
            {
                "abcd@via.dk",
                "XYZ@via.dk",
                "AbCd@via.dk",
                "123456@via.dk",
                "987654@via.dk",
                "QweR@via.dk",
                "AAA@via.dk",
                "BbCc@via.dk",
                "765432@via.dk"
            };
        }
    public Task<bool> IsEmailUnique(string email)
    {
        return Task.FromResult(!emails.Contains(email.ToLower()));
    }
}