using Binance.Net;
using Binance.Net.Objects;
using BinanceDCA.Models;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BinanceDCA
{
    class Program
    {
        static void Main()
        {
            var configurationRoot = Configuration.Build();
            
            var config = configurationRoot.Get<Config>();

            Log.Logger = Configuration.BuildLogger(configurationRoot);
            
            var host = CreateDefaultBuilder(config).Build();
            
            using IServiceScope serviceScope = host.Services.CreateScope();
            // IServiceProvider provider = serviceScope.ServiceProvider;
            
            Log.Logger.Information("Starting");
            host.Run();
        }

        static IHostBuilder CreateDefaultBuilder(Config config)
        {
            return Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(config);
                    
                    var client = new BinanceClient(new BinanceClientOptions(){
                        ApiCredentials = new ApiCredentials(config.Binance.APIKey, config.Binance.APISecret)
                    });
                    services.AddSingleton(client);
                    
                    QuartzConfiguration.Setup(services, config);
                });
        }
    }
}