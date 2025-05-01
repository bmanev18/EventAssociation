using EventAssociation.Core.QueryContracts.Contract;

namespace EventAssociation.Core.QueryContracts.QueryDispatching;

public interface IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}