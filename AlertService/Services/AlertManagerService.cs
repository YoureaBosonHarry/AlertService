using AlertService.Services.Interfaces;
using AlertService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace AlertService.Services
{
    class AlertManagerService : IAlertManagerService
    {
        private readonly IIndicatorsHttpService indicatorsHttpService;
        private System.Threading.Timer timer;

        public AlertManagerService(IIndicatorsHttpService indicatorsHttpService)
        {
            this.indicatorsHttpService = indicatorsHttpService;
        }
        
        public void SetTimer(TimeSpan alertTime)
        {
            TimeSpan timeRemaining = alertTime - DateTime.UtcNow.TimeOfDay;
            if (timeRemaining < TimeSpan.Zero)
            {
                return;
            }
            this.timer = new System.Threading.Timer(async x =>
            {
                await this.ManageAlerts();
            }, null, timeRemaining, Timeout.InfiniteTimeSpan);
        }

        private async Task ManageAlerts()
        {
            await this.ManageRSIAlert();
        }

        private async Task ManageRSIAlert()
        {
            await this.indicatorsHttpService.InsertDailyRSIAsync();
            var rsiModel = await this.indicatorsHttpService.GetDailyRSIAsync();
            var rsiOfInterest = rsiModel.Where(i => i.FourteenDayRsi < 30 && i.FourteenDayRsi > 0).OrderBy(j => j.FourteenDayRsi);
            foreach (var rsi in rsiOfInterest)
            {
                Console.WriteLine($"{rsi.Ticker} {rsi.FourteenDayRsi}");
            }
        }
    }
}
