using EventAssociation.Core.QueryContracts.Contract;

namespace EventAssociation.Core.QueryContracts.QueryDispatching;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        Type queryInterfaceWithTypes = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TAnswer));
        dynamic handler = serviceProvider.GetService(queryInterfaceWithTypes);

        if (handler == null)
        {
            throw new Exception($"Query type {queryInterfaceWithTypes.Name} does not implement {typeof(IQueryHandler<,>)}");
        }
        return handler.HandleAsync((dynamic)query);
    }
}