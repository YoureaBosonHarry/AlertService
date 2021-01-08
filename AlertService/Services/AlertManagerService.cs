using AlertService.HttpService.Interfaces;
using AlertService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AlertService.HttpService
{
    class AlertManagerService : IAlertManagerService
    {
        private readonly IIndicatorsHttpService indicatorsHttpService;
        private readonly int waitTime;

        public AlertManagerService(IIndicatorsHttpService indicatorsHttpService)
        {
            this.indicatorsHttpService = indicatorsHttpService;
            this.waitTime = 1000 * 60 * 60 * 24;
        }

        public async Task SetTimer()
        {
            await this.ManageAlerts();
            var alertCheck = new System.Timers.Timer(this.waitTime);
            alertCheck.Elapsed += (a, b) => { ManageAlerts().Wait(); };
            alertCheck.Start();
        }

        private async Task ManageAlerts()
        {
            await this.ManageRSIAlert();
        }

        private async Task ManageRSIAlert()
        {
            await this.indicatorsHttpService.InsertDailyRSIAsync();
            var rsiModel = await this.indicatorsHttpService.GetDailyRSIAsync();
        }
    }
}
