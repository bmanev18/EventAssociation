using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common;

public interface IUnitOfWork
{
    public Task<Result<None>> SaveChangesAsync();
}