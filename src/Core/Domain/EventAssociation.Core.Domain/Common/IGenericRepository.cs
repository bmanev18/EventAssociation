using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common;

public interface IGenericRepository<TAggr, in TId> where TAggr: AggregateRoot 
{
    Task<Result<TAggr>> GetAsync(TId id);
    Task<Result<None>> RemoveAsync(TId id);
    Task<Result<None>> AddAsync(TAggr aggregate);
}