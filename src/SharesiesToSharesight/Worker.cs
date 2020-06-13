using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesiesClient;
using SharesiesToSharesight.SharesightClient;
using SharesiesToSharesight.SharesightClient.Models;

namespace SharesiesToSharesight
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISharesightClient _sharesightClient;
        private readonly ISharesiesClient _sharesiesClient;
        private readonly SharesightTransactionConverter _sharesightTransactionConverter;

        public Worker(ILogger<Worker> logger, ISharesiesClient sharesiesClient, ISharesightClient sharesightClient,SharesightTransactionConverter sharesightTransactionConverter)
        {
            _logger = logger;
            _sharesiesClient = sharesiesClient;
            _sharesightClient = sharesightClient;
            _sharesightTransactionConverter = sharesightTransactionConverter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _sharesightTransactionConverter.Symbols = await _sharesiesClient.GetSymbolsAsync();
                    var history = await _sharesiesClient.GetPaymentHistoryAsync();
                    for (var i = 0; i < history.Transactions.Length; i++)
                    {
                        var trades = _sharesightTransactionConverter.Convert(history.Transactions[i]);
                        foreach (var trade in trades)
                        {
                            if (await _sharesightClient.GetMatchingTransactionAsync(trade))
                            {
                                continue;
                            }
                            await _sharesightClient.AddTradeAsync(trade);
                        }


                        if (i != history.Transactions.Length - 1 || !history.HasMore)
                        {
                            continue;
                        }

                        history = await _sharesiesClient.GetPaymentHistoryAsync(history.Transactions[i].TransactionId);
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