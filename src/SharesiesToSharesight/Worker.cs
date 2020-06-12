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
        private readonly ISharesightClient _sharesightClient;
        private readonly ISharesiesClient _sharesiesClient;

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
                try
                {
                    var history = await _sharesiesClient.GetPaymentHistoryAsync();
                    for (var i = 0; i < history.Transactions.Length; i++)
                    {
                        var transaction = history.Transactions[i];
                        //if (_sharesightClient.GetMatchingTransaction(transaction))
                        //{
                        //    break;
                        //}
                        //_sharesightClient.AddTransaction(transaction);

                        if (i != history.Transactions.Length - 1 || !history.HasMore)
                        {
                            continue;
                        }

                        history = await _sharesiesClient.GetPaymentHistoryAsync(transaction.TransactionId);
                        i = 0;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }

                await Task.Delay((int)TimeSpan.FromHours(1).TotalMilliseconds, stoppingToken);
            }
        }
    }
}