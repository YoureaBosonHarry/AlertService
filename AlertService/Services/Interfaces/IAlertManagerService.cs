﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertService.Services.Interfaces
{
    public interface IAlertManagerService
    {
        Task SetTimer();
    }
}
