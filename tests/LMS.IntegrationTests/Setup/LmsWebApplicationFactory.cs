using LMS.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LMS.IntegrationTests.Setup;

public class LmsWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string DbName = "LmsTestDb";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LmsDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<LmsDbContext>(options =>
            {
                options.UseInMemoryDatabase(DbName);
            });

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LmsDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
