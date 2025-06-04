using EventAssociation.Infrastructure.EfcQueries.Models;
using EventAssociation.Infrastructure.SqliteDmPersistence;
using IntegrationTests.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTests.Factories;

internal class VeaWebApplicationFactory : WebApplicationFactory<Program>
{
    private IServiceCollection _serviceCollection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _serviceCollection = services;

            services.RemoveAll(typeof(DbContextOptions<DmContext>));
            services.RemoveAll(typeof(DbContextOptions<EventAssociationProductionContext>));
            services.RemoveAll<DmContext>();
            services.RemoveAll<EventAssociationProductionContext>();

            string connString = GetConnectionString();

            services.AddDbContext<DmContext>(options => { options.UseSqlite(connString); });
            services.AddDbContext<EventAssociationProductionContext>(options => { options.UseSqlite(connString); });

            SetupFreshDb(services);
        });
    }

    private string GetConnectionString()
    {
        string testDbName = "Test" + Guid.NewGuid() + ".db";
        return "Data Source = " + testDbName;
    }

    private void SetupFreshDb(IServiceCollection services)
    {
        DmContext dmContext = services.BuildServiceProvider().GetService<DmContext>()!;
        dmContext.Database.EnsureDeleted();
        dmContext.Database.EnsureCreated();
    }

    protected override void Dispose(bool disposing)
    {
        DmContext context = _serviceCollection.BuildServiceProvider().GetService<DmContext>()!;
        context.Database.EnsureDeleted();
        base.Dispose(disposing);
    }
}