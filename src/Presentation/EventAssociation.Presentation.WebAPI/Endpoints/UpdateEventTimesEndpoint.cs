using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionResultHelper;
using Endpoint = WebAPI.REPRBase.Endpoint;

namespace WebAPI.Endpoints;

public class UpdateEventTimesEndpoint(ICommandDispatcher dispatcher)
    : Endpoint.WithRequest<UpdateEventTimesEndpointRequest>.WithoutResponse
{
    [HttpPatch("events/update-times")]
    public override async Task<ActionResult> HandleAsync(UpdateEventTimesEndpointRequest request)
    {
        var commandResult = UpdateEventTimesCommand.Create(
            request.RequestBody.EventId,
            request.RequestBody.StartDateTime,
            request.RequestBody.EndDateTime
        );

        if (!commandResult.IsSuccess)
        {
            return ActionResultConverter.FromErrors(commandResult.UnwrapErr());
        }

        var handlerResult = await dispatcher.DispatchAsync(commandResult.Unwrap());

        if (!handlerResult.IsSuccess)
        {
            return ActionResultConverter.FromErrors(handlerResult.UnwrapErr());
        }

        return Ok();
    }
}


public record UpdateEventTimesEndpointRequest(
    [FromBody] UpdateEventTimesEndpointRequest.Body RequestBody
)
{
    public record Body(
        string EventId,
        string StartDateTime,
        string EndDateTime
    );
}
