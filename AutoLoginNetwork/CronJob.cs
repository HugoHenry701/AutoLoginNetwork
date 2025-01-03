using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoLoginNetwork
{
    public class CronJob: BackgroundService
    {
        private readonly CronExpression _cronExpression;
        private DateTimeOffset? _nextRun;
        private CancellationTokenSource _cancellationTokenSource;

        public CronJob(string cronExpression)
        {
            _cronExpression = CronExpression.Parse(cronExpression);
            _cancellationTokenSource = new CancellationTokenSource();
            _nextRun = _cronExpression.GetNextOccurrence(DateTime.UtcNow);
        }
        public async Task StartAsync(string username, string password)
        {
            Console.WriteLine("[EVENT] Auto Login Network start!");
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var next = _cronExpression.GetNextOccurrence(DateTime.UtcNow);
                if (next.HasValue)
                {
                    var delay = next.Value - DateTime.UtcNow;
                    if (delay.TotalMilliseconds > 0)
                    {
                        Handler.AutoLoginNetwork(username, password);
                        await Task.Delay(delay, _cancellationTokenSource.Token);
                        Console.WriteLine($"Job executed at: {DateTime.UtcNow}");
                    }
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTimeOffset.Now;
                if (_nextRun.HasValue && now >= _nextRun)
                {
                    //Handler.AutoLoginNetwork();
                    _nextRun = _cronExpression.GetNextOccurrence(DateTime.UtcNow);
                }

                await Task.Delay(1000, stoppingToken); // Check every second
            }
        }

        
    }
}
