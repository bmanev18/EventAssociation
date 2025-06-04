using System.Net;
using System.Net.Http.Json;
using EventAssociation.Infrastructure.SqliteDmPersistence;
using IntegrationTests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebAPI.Endpoints;
using Xunit;

namespace IntegrationTests.EndpointTests;

public class RegisterGuestEndpointTest
{
    private readonly HttpClient _client;

    [Fact]
    public async Task RegisterGuestEndpoint_Should_RegisterGuestSuccessfully()
    {
        // Arrange
        await using WebApplicationFactory<Program> webApplicationFactory = new VeaWebApplicationFactory();
        HttpClient client = webApplicationFactory.CreateClient();

        var request = new RegisterGuestEndpointRequest(
            new RegisterGuestEndpointRequest.Body(
                FirstName: "Michael",
                LastName: "Jackson",
                Email: "mjmj@via.dk",
                ImageUrl: "https://example.com/mj.jpg"
            )
        );

        // Act
        HttpResponseMessage responseMessage = await client.PostAsync(
            "/api/guest/register",
            JsonContent.Create(request)
        );

        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<DmContext>();
        var guests = context.Guests.ToList();

        // Assert
        Assert.True(responseMessage.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        Assert.Single(guests);
    }
}