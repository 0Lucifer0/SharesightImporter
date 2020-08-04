using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Exporter;
using SharesightImporter.SharesightClient;
using SharesightImporter.SharesightClient.Models;

namespace SharesightImporter
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISharesightClient _sharesightClient;
        private readonly IEnumerable<IExporterClient> _exporters;

        public Worker(ILogger<Worker> logger, IEnumerable<IExporterClient> exporters, ISharesightClient sharesightClient)
        {
            _logger = logger;
            _exporters = exporters;
            _sharesightClient = sharesightClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach(var exporter in _exporters.ToList())
                    {
                        var tradeHistory = await _sharesightClient.GetTradeHistoryAsync(exporter.PortfolioId);

                        var trades = await exporter.GetTrades();
                        foreach (var trade in trades)
                        {
                            if (tradeHistory.Trades.Any(s => (s.UniqueIdentifier == trade.UniqueIdentifier && trade.UniqueIdentifier != null) || (s.Symbol == trade.Symbol && s.Quantity == trade.Quantity && s.Price == trade.Price && s.TransactionDate.Date == trade.TransactionDate.Date)))
                            {
                                _logger.LogDebug("Matching trade found! {0} {1} {2} {3}", trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                                continue;
                            }
                            _logger.LogDebug("Matching trade not found! {0} {1} {2} {3}", trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                            await _sharesightClient.AddTradeAsync(trade);
                        }
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