using AlertService.HttpService.Interfaces;
using AlertService.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace AlertService.HttpService
{
    public class IndicatorsHttpService : IIndicatorsHttpService
    {
        private readonly string taInfoEndpoint;

        public IndicatorsHttpService(string taInfoEndpoint)
        {
            this.taInfoEndpoint = taInfoEndpoint;
        }


        public async Task<IEnumerable<RsiModel>> GetDailyRSIAsync() 
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{this.taInfoEndpoint}/TechnicalAnalysis/GetRSI");
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStreamAsync();
                var jsonOptions = new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var rsiModel = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<RsiModel>>(resp, jsonOptions);
                return rsiModel;
            }
        }

        public async Task InsertDailyRSIAsync()
        {
           using (var client = new HttpClient())
            {
                var tickers = await this.GetTickersAsync();
                foreach (var ticker in tickers)
                {
                    var jsonOptions = new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var payload = System.Text.Json.JsonSerializer.Serialize<string>(ticker.Ticker, jsonOptions);
                    await client.PostAsync($"{this.taInfoEndpoint}/TechnicalAnalysis/InsertRSI", new StringContent(payload, Encoding.Default, "application/json"));
                }
            }
        }

        public async Task<IEnumerable<Tickers>> GetTickersAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{this.taInfoEndpoint}/TickerInfo/GetTickers");
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStreamAsync();
                var jsonOptions = new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var tickers = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<Tickers>>(resp, jsonOptions);
                return tickers;
            }
        }
    }
}
