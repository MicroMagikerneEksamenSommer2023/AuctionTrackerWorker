using AuctionTrackerWorker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using AuctionTrackerWorker.Services;
using NLog;
using NLog.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<MongoService>();  // Register the MongoService

    services.AddSingleton<IMongoServiceFactory>(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new MongoServiceFactory(scopeFactory);
});

        // Add MVC services
        services.AddMvc();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure(app =>
        {
            // Add MVC middleware
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Map your MVC routes
                endpoints.MapControllers();
            });
        });
    })
    .UseNLog()
    .Build();

host.Run();