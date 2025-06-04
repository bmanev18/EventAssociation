using EventAssociation.Core.Application;
using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Application.CommandDispatching.Features;
using EventAssociation.Core.Domain.Aggregates.Event;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Core.Domain.Aggregates.Guests.Contracts;
using EventAssociation.Core.Domain.Common;
using EventAssociation.Core.QueryContracts.QueryDispatching;
using EventAssociation.Infrastructure.SqliteDmPersistence;
using EventAssociation.Infrastructure.SqliteDmPersistence.Repositories;
using EventAssociation.Infrastructure.SqliteDmPersistence.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Extensions;

public static class ServicesExtensions
{
    public static void RegisterDatabase(this IServiceCollection services)
    {
        const string dbName = @"/home/dzx/Documents/GitHub/EventAssociation/src/Infrastructure/EventAssociation.Infrastructure.SqliteDmPersistence/EventAssociationProduction.db";
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<DmContext>();
        dbContextOptionsBuilder.UseSqlite($"Data Source={dbName}");

        DmContext context = new(dbContextOptionsBuilder.Options);

        services.AddDbContext<DmContext>(options =>
            options.UseSqlite($"Data Source={dbName}"));
    }
    
    public static void RegisterDispatcher(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, Dispatch>();
        // services.AddScoped<ICommandDispatcher, CommandExecutionTimerDispatcher>();
        // services.AddScoped<Dispatch>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
    
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEmailChecker, EmailChecker>();
        services.AddScoped<ICommandHandler<RegisterANewGuestCommand>, RegisterANewGuestHandler>();

    }
    
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

    }

    public static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfcUnitOfWork>();
    }
}