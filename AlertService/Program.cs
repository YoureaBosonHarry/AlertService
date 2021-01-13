using AlertService.Services;
using AlertService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace AlertService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var taServiceEndpoint = Environment.GetEnvironmentVariable("TASERVICEENDPOINT");
            var sendgridApiKey = Environment.GetEnvironmentVariable("SENDGRIDAPIKEY");
            var fromAddress = Environment.GetEnvironmentVariable("FROMADDRESS");
            var fromName = Environment.GetEnvironmentVariable("FROMNAME");
            var indicatorHttpService = new IndicatorsHttpService(taServiceEndpoint);
            var serviceProvider = new ServiceCollection()
                .AddScoped<IIndicatorsHttpService>(_ => indicatorHttpService)
                .AddScoped<IAlertManagerService>(_ => new AlertManagerService(indicatorHttpService))
                .AddScoped<IAlertSenderService>(_ => new AlertSenderService(sendgridApiKey, fromAddress, fromName))
                .BuildServiceProvider();

            var senderService = serviceProvider.GetService<IAlertSenderService>();
            await senderService.SendEmail("null@yahoo.com");
            //var alertService = serviceProvider.GetService<IAlertManagerService>();
            //await alertService.SetTimer();
            Task.Delay(-1).Wait();
        }
    }
}
