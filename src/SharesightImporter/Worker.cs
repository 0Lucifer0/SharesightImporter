using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesightImporter.Exporter;
using SharesightImporter.Importer;

namespace SharesightImporter
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEnumerable<IImporterClient> _importers;
        private readonly IEnumerable<IExporterClient> _exporters;

        public Worker(ILogger<Worker> logger, IEnumerable<IExporterClient> exporters, IEnumerable<IImporterClient> importers)
        {
            _logger = logger;
            _exporters = exporters;
            _importers = importers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var exporter in _exporters.OrderBy(o => o.Order))
                {
                    foreach (var importer in _importers)
                    {
                        try
                        {
                            var tradeHistory = await importer.GetTradeHistoryAsync(exporter.PortfolioId);

                            var trades = await exporter.GetTrades();

                            var grouped = trades.GroupBy(o => o.UniqueIdentifier).ToList();
                            var uniqueTrades = grouped.Where(x => x.Key == null).SelectMany(x => x.ToList()).ToList();
                            uniqueTrades.AddRange(grouped.Where(x => x.Key != null).Select(x => x.First()));
                            var importerName = importer.GetType().Name.Replace("ImporterClient", "");
                            foreach (var trade in uniqueTrades.OrderBy(o => o.TransactionDate.Date)
                                .ThenByDescending(o => o.TransactionType == "BUY")
                                .ThenByDescending(o => o.TransactionType == "BONUS").ToList())
                            {
                                if (tradeHistory.Any(s =>
                                    (s.UniqueIdentifier == trade.UniqueIdentifier && trade.UniqueIdentifier != null) ||
                                    (s.Symbol == trade.Symbol && s.Quantity == trade.Quantity &&
                                     Math.Round(s.Price ?? 0d, 3) == Math.Round(trade.Price ?? 0d, 3) &&
                                     s.TransactionDate.Date == trade.TransactionDate.Date)))
                                {
                                    _logger.LogInformation("Matching trade found in {0}! {1} {2} {3} {4} {5}", importerName, trade.TransactionType,
                                        trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                                    continue;
                                }

                                _logger.LogInformation("Matching trade not found in {0}! {1} {2} {3} {4} {5}", importerName, trade.TransactionType,
                                trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                                try
                                {
                                    await importer.AddTradeAsync(trade);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("Adding new trade in {0} failed! {1} {2} {3} {4} {5}", importerName, trade.TransactionType,
                                        trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date, ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex);
                        }
                    }
                }

                _logger.LogInformation("All trades imported!");
                await Task.Delay((int)TimeSpan.FromHours(1).TotalMilliseconds, stoppingToken);
            }
        }
    }
}