using AlertService.HttpService;
using AlertService.HttpService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace AlertService
{
    class Program
    {
        //private static Timer aTimer;

        public static async Task Main(string[] args)
        {
            var taServiceEndpoint = Environment.GetEnvironmentVariable("TASERVICEENDPOINT");
            var indicatorHttpService = new IndicatorsHttpService(taServiceEndpoint);
            var serviceProvider = new ServiceCollection()
           .AddScoped<IIndicatorsHttpService>(_ => indicatorHttpService)
           .AddScoped<IAlertManagerService>(_ => new AlertManagerService(indicatorHttpService))
           .BuildServiceProvider();

            var alertService = serviceProvider.GetService<IAlertManagerService>();
            await alertService.SetTimer();
            Task.Delay(-1).Wait();
        }
    }
}
