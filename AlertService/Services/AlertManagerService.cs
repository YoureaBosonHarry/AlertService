using AlertService.Services.Interfaces;
using AlertService.Models;
using Serilog;
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
        private readonly ILogger logger;

        public AlertManagerService(IIndicatorsHttpService indicatorsHttpService)
        {
            this.indicatorsHttpService = indicatorsHttpService;
            this.logger = Log.ForContext<AlertManagerService>();
        }
        
        public void SetTimer(TimeSpan alertTime)
        {
            TimeSpan timeRemaining = alertTime - DateTime.Now.TimeOfDay;
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
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                this.logger.Information($"{DateTime.Now.DayOfWeek}: Market Closed");
                return;
            }
            this.logger.Information($"{DateTime.Now.DayOfWeek}: Sending Alerts");
            await this.ManageRSIAlert();
        }

        private async Task ManageRSIAlert()
        {
            await this.indicatorsHttpService.InsertDailyRSIAsync();
            var rsiModel = await this.indicatorsHttpService.GetDailyRSIAsync();
            var rsiOfInterest = rsiModel.Where(i => i.FourteenDayRsi < 30 && i.FourteenDayRsi > 0).OrderBy(j => j.FourteenDayRsi);
        }
    }
}
