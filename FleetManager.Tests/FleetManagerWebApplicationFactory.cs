using FleetManager.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.Tests;

public class FleetManagerWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=admin;Database=FleetManager"); // TODO: add test database
            });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }
        });

        base.ConfigureWebHost(builder);
    }
}
