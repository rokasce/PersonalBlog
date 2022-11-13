using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using DotNet.Testcontainers.Configurations;

namespace API.Tests.Integration;

public class BlogApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _dbContainer = 
        new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "cashmeoutside420BlazeItHopeItsGoodEnough..."
            })
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithCleanUp(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DataContext));
            if (context != null)
            {
                services.Remove(context);
                var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                  || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(_dbContainer.ConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
