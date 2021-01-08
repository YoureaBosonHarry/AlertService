using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertService.HttpService.Interfaces
{
    interface IAlertManagerService
    {
        Task SetTimer();
    }
}
