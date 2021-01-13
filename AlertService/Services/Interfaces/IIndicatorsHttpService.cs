using AlertService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertService.Services.Interfaces
{
    public interface IIndicatorsHttpService
    {
        Task<IEnumerable<RsiModel>> GetDailyRSIAsync();
        Task InsertDailyRSIAsync();
    }
}
