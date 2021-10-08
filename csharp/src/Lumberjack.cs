using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsDemo
{
    public class Lumberjack : BackgroundService
    {
        private readonly ILogger<Lumberjack> _logger;

        public Lumberjack(ILogger<Lumberjack> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Lumberjack logging at: {time} and here's a Guid for you: {randomGuid}", DateTimeOffset.Now, Guid.NewGuid());
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
