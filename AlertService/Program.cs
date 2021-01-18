using AlertService.Services;
using AlertService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace AlertService
{
    class Program
    {
        public static void Main(string[] args)
        {
            var taServiceEndpoint = Environment.GetEnvironmentVariable("TASERVICEENDPOINT");
            var sendgridApiKey = Environment.GetEnvironmentVariable("SENDGRIDAPIKEY");
            var fromAddress = Environment.GetEnvironmentVariable("FROMADDRESS");
            var fromName = Environment.GetEnvironmentVariable("FROMNAME");
            var indicatorHttpService = new IndicatorsHttpService(taServiceEndpoint);

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddScoped<IIndicatorsHttpService>(_ => indicatorHttpService)
                .AddScoped<IAlertManagerService>(_ => new AlertManagerService(indicatorHttpService))
                .AddScoped<IAlertSenderService>(_ => new AlertSenderService(sendgridApiKey, fromAddress, fromName))
                .BuildServiceProvider();

            //var senderService = serviceProvider.GetService<IAlertSenderService>();
            //await senderService.SendEmail("benjamin.rathbone@yahoo.com");
            var alertService = serviceProvider.GetService<IAlertManagerService>();
            alertService.SetTimer(new TimeSpan(16, 30, 0));
            Task.Delay(-1).Wait();
        }
    }
}
