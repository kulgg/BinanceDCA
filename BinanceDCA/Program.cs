using System;
using System.Threading.Tasks;
using BinanceDCA.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BinanceDCA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurationRoot = Configuration.Build();
            
            var config = configurationRoot.Get<Config>();

            Log.Logger = Configuration.BuildLogger(configurationRoot);
            
            var host = CreateDefaultBuilder(config).Build();
            
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            
            var app = provider.GetRequiredService<App>();
            await app.Execute();
        }

        static IHostBuilder CreateDefaultBuilder(Config config)
        {
            return Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton(config);
                });
        }
    }
}