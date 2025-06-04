using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionResultHelper;
using Endpoint = WebAPI.REPRBase.Endpoint;

namespace WebAPI.Endpoints;

public class RegisterGuestEndpoint(ICommandDispatcher dispatcher)
    : Endpoint.WithRequest<RegisterGuestEndpointRequest>.WithoutResponse
{
    [HttpPost("guest/register")]
    public override async Task<ActionResult> HandleAsync(RegisterGuestEndpointRequest request)
    {
        var command = RegisterANewGuestCommand.Create(request.RequestBody.FirstName, request.RequestBody.LastName,
            request.RequestBody.Email, request.RequestBody.ImageUrl);

        if (!command.IsSuccess)
        {
            return ActionResultConverter.FromErrors(command.UnwrapErr());
        }

        var handlerResult = await dispatcher.DispatchAsync(command.Unwrap());

        if (!handlerResult.IsSuccess)
        {
            return ActionResultConverter.FromErrors(handlerResult.UnwrapErr());
        }

        return Ok();
    }
}

public record RegisterGuestEndpointRequest([FromBody] RegisterGuestEndpointRequest.Body RequestBody)
{
    public record Body(
        string FirstName,
        string LastName,
        string Email,
        string ImageUrl
    );
}