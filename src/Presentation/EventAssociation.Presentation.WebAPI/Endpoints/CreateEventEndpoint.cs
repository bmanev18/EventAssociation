using EventAssociation.Core.Application.CommandDispatching.Commands;
using Microsoft.AspNetCore.Mvc;
using Endpoint = WebAPI.REPRBase.Endpoint;

namespace WebAPI.Endpoints;

public class CreateEventEndpoint : Endpoint.WithRequest<CreateEventEndpointRequest>.WithoutResponse
{


    public override Task<ActionResult> HandleAsync(CreateEventEndpointRequest request)
    {
        throw new NotImplementedException();
    }
}

public record CreateEventEndpointRequest()
{
    
}


