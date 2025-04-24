using EventAssociation.Core.Tools.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EventAssociation.Infrastructure.SqliteDmPersistence.Shared;

public class RepositoryBase<TAggr, TId>(DbContext context) : IGenericRepository<TAggr, TId> where TAggr : AggregateRoot
{
    public static Error EntityNotFound() => new Error("0", "Entity not found in the database");

    public virtual async Task<Result<TAggr>> GetAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result<TAggr>.Err(new Error(EntityNotFound().Code, EntityNotFound().Message));

        }

        return Result<TAggr>.Ok(root);
    }

    public virtual async Task<Result<None>> RemoveAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result<None>.Err(new Error(EntityNotFound().Code, EntityNotFound().Message));
        }

        context.Set<TAggr>().Remove(root);

        return Result<None>.Ok(None.Value);
    }

    public virtual async Task<Result<None>> AddAsync(TAggr aggregate)
    {
        await context.Set<TAggr>().AddAsync(aggregate);
        return Result<None>.Ok(None.Value);
        
    }
}

public interface IGenericRepository<T, T1>
{
}