using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task<Result<None>> SaveChangesAsync()
    {
        return Task.FromResult(Result<None>.Ok(None.Value));
    }
}