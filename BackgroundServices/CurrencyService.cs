using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CCostsProject.BackgroundServices
{
    public class CurrencyService:IHostedService,IDisposable
    {
        private readonly ILogger<CurrencyService> _logger;
        private Timer _timer;
        private IWorker currencyWorker;
        
        public CurrencyService(ILogger<CurrencyService> logger,ApplicationContext db)
        {
            _logger = logger;
            currencyWorker=new CurrencyWorker(db);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Currency service is running");
            _timer=new Timer(DoWork,null,TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Currency service is stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void  DoWork(object state)
        {
            HttpClient client=new HttpClient();
            HttpRequestMessage request=new HttpRequestMessage();
            request.RequestUri=new Uri($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");

            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var test = responseContent.ReadAsStringAsync().Result;

                var deserializedJson =
                    JsonConvert.DeserializeObject<List<Currency>>(responseContent.ReadAsStringAsync().Result);
                deserializedJson.InsertRange(1,new List<Currency>{deserializedJson.First(cur=>cur.cc=="USD"),
                    deserializedJson.First(cur=>cur.cc=="EUR"),
                    new Currency{exchangedate = DateTime.Now.ToString(),cc="UAH",txt = "Українська гривня",rate = 1}});
                
                if (!currencyWorker.GetEntities().Any())
                {
                    foreach (var c in deserializedJson.Distinct())
                    {
                        currencyWorker.AddEntity(c);
                        _logger.LogInformation($"{c.txt} was added to db");
                    }
                }
                else
                {
                    foreach (var c in deserializedJson)
                    {
                        var currentCur = currencyWorker.GetEntities().Cast<Currency>().FirstOrDefault(t => t.cc == c.cc);
                        if (currentCur != null)
                        {
                            c.Id = currentCur.Id;
                            currencyWorker.EditEntity(c);
                        }
                    }
                }
                
                
            }

                       
        }
    }
}