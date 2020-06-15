using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CConstsProject.Models;
using CCostsProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CCostsProject.BackgroundServices
{
    public class GoalService:IHostedService,IDisposable
    {
        private readonly ILogger<CurrencyService> _logger;
        private Timer _timer;
        private IWorker goalWorker;
        private readonly IServiceScopeFactory serviceScope;
        public GoalService(ILogger<CurrencyService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            serviceScope = scopeFactory;
            //goalWorker=new PlanWorker(db);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Goal service is running");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var scope = serviceScope.CreateScope();
            goalWorker =new PlanWorker(scope.ServiceProvider.GetRequiredService<ApplicationContext>());
            var goals = goalWorker.GetEntities().Cast<Plan>();
            foreach (var g in goals.Where(g => g.Status == "Active")) 
            {
                
                if (g.User.Money < g.Money && g.DateFinish < DateTime.Now)
                {
                    ((PlanWorker) goalWorker).ChangeStatus("Failed", g.Id);
                    _logger.LogInformation($"Goal {g.Id} failed");
                }else if (g.User.Money >= g.Money && g.DateFinish < DateTime.Now)
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