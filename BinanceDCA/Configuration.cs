using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace BinanceDCA
{
    public static class Configuration
    {
        public static IConfigurationRoot Build()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.local.json", true)
                .Build();
        }

        public static Logger BuildLogger(IConfigurationRoot configurationRoot)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}