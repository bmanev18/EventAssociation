using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using EventAssociation.Core.Application.CommandDispatching.Features;
using EventAssociation.Core.Domain.Aggregates.Guests;
using EventAssociation.Infrastructure.SqliteDmPersistence.Repositories;
using Microsoft.AspNetCore.Components;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Refactor the following to go in methods in Presentation/Extensions
builder.Services.AddScoped<ICommandDispatcher, Dispatch>();
builder.Services.AddScoped<ICommandHandler<RegisterANewGuestCommand>, RegisterANewGuestHandler>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>(); 



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();