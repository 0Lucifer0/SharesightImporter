using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesightImporter.Exporter;
using SharesightImporter.SharesightClient;

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
                foreach (var exporter in _exporters.OrderBy(o=>o.Order))
                {
                    try
                    {
                        var tradeHistory = await _sharesightClient.GetTradeHistoryAsync(exporter.PortfolioId);

                        var trades = await exporter.GetTrades();
                        foreach (var trade in trades.OrderBy(o => o.TransactionDate.Date).ThenByDescending(o=>o.TransactionType == "BUY").ThenByDescending(o => o.TransactionType == "BONUS").ToList())
                        {
                            if (tradeHistory.Trades.Any(s => (s.UniqueIdentifier == trade.UniqueIdentifier && trade.UniqueIdentifier != null) || (s.Symbol == trade.Symbol && s.Quantity == trade.Quantity && Math.Round(s.Price ?? 0d, 3) == Math.Round(trade.Price ?? 0d, 3) && s.TransactionDate.Date == trade.TransactionDate.Date)))
                            {
                                _logger.LogDebug("Matching trade found! {0} {1} {2} {3} {4}", trade.TransactionType, trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                                continue;
                            }
                            _logger.LogDebug("Matching trade not found! {0} {1} {2} {3} {4}", trade.TransactionType, trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                            await _sharesightClient.AddTradeAsync(trade);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                }

                _logger.LogInformation("All trades imported!");
                await Task.Delay((int)TimeSpan.FromHours(1).TotalMilliseconds, stoppingToken);
            }
        }
    }
}