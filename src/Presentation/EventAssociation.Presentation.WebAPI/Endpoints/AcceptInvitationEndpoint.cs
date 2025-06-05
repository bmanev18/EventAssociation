using EventAssociation.Core.Application.CommandDispatching;
using EventAssociation.Core.Application.CommandDispatching.Commands;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionResultHelper;
using Endpoint = WebAPI.REPRBase.Endpoint;

namespace WebAPI.Endpoints;

public class AcceptInvitationEndpoint(ICommandDispatcher dispatcher)
    : Endpoint.WithRequest<AcceptInvitationEndpointRequest>.WithoutResponse
{
    [HttpPost("invitations/{InvitationId}/accept")]
    public override async Task<ActionResult> HandleAsync(AcceptInvitationEndpointRequest request)
    {
        var commandResult = AcceptInvitationCommand.Create(request.InvitationId);

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
public record AcceptInvitationEndpointRequest([FromRoute] string InvitationId);
