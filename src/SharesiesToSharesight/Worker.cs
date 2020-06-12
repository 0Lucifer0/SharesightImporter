using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesiesClient;
using SharesiesToSharesight.SharesightClient;

namespace SharesiesToSharesight
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ISharesightClient _sharesightClient;
        private ISharesiesClient _sharesiesClient;

        public Worker(ILogger<Worker> logger, ISharesiesClient sharesiesClient, ISharesightClient sharesightClient)
        {
            _logger = logger;
            _sharesiesClient = sharesiesClient;
            _sharesightClient = sharesightClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _sharesiesClient.GetPayments();

                await Task.Delay((int)TimeSpan.FromHours(1).TotalMilliseconds, stoppingToken);
            }
        }
    }
}