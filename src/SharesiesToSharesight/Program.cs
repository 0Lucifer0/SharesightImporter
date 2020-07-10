using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SharesightImporter.SharesiesClient;
using SharesightImporter.SharesightClient;

namespace SharesightImporter
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new Configuration.Configuration();
            new ConfigurationBuilder().AddYamlFile("config.yml").Build().Bind(configuration);
            Validator.ValidateObject(configuration, new ValidationContext(configuration), true);
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Level:u4} {Timestamp:HH:mm:ss} -- {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Debug()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger();
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging(
                    loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddSerilog(dispose: true);
                    }
                )
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(configuration);
                    services.AddTransient<SharesightTransactionConverter>();
                    services.AddHttpClient();
                    services.AddTransient<ISharesiesClient, SharesiesClient.SharesiesClient>();
                    services.AddTransient<ISharesightClient, SharesightClient.SharesightClient>();
                    services.AddHostedService<Worker>();
                }); 
        }
    }
}
