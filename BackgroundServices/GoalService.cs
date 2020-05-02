﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CCostsProject.BackgroundServices
{
    public class GoalService:IHostedService,IDisposable
    {
        private readonly ILogger<CurrencyService> _logger;
        private Timer _timer;
        private IWorker goalWorker;

        public GoalService(ILogger<CurrencyService> logger,ApplicationContext db)
        {
            _logger = logger;
            goalWorker=new PlanWorker(db);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Goal service is running");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(20));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var goals = goalWorker.GetEntities().Cast<Plan>();
            foreach (var g in goals.Where(g => g.Status == "Active")) 
            {
                
                if (g.User.CashSum < g.Money && g.DateFinish < DateTime.Now)
                {
                    ((PlanWorker) goalWorker).ChangeStatus("Failed", g.Id);
                    _logger.LogInformation($"Goal {g.Id} failed");
                }else if (g.User.CashSum >= g.Money && g.DateFinish < DateTime.Now)
                {
                    ((PlanWorker) goalWorker).ChangeStatus("Success", g.Id);
                    _logger.LogInformation($"Goal {g.Id} succeed");

                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Goal service is stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}