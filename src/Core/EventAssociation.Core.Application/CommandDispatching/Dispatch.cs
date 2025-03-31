using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.CommandDispatching;

public class Dispatch : ICommandDispatcher
{
    //TODO: Need to fix setup required in promgram.cs for webapi (services.AddScoped/Decorate)
    /*
      services.Scan(scan => scan
    .FromAssembliesOf(typeof(DeclineInvitationHandler))  // The type of any of your handlers
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
     */
    private readonly IServiceProvider _serviceProvider;

    public Dispatch(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;

        if (handler == null)
        {
            return Result<None>.Err(new Error("100", $"No handler found for command {typeof(TCommand).Name}"));
        }

        return await handler.HandleAsync(command);
    }
}