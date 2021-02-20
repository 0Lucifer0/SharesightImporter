using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Exporter.CsvExporter
{
    public class CsvExporterClient : IExporterClient
    {
        public int Order => 1;
        public string PortfolioId => _configuration.PortfolioId;
        private readonly ILogger<CsvExporterClient> _logger;
        private readonly CsvExporterConfiguration _configuration;

        public CsvExporterClient(ILogger<CsvExporterClient> logger, CsvExporterConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<List<TradePost>> GetTrades()
        {
            using var reader = new StreamReader(_configuration.Path);
            var tradeList = new List<TradePost>();
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = _ => false,
            });
            var record = new CsvTrade();
            var records = csv.EnumerateRecordsAsync(record);
            await foreach (var r in records)
            {
                tradeList.Add(new TradePost
                {
                    Quantity = r.Quantity,
                    Market = r.MarketCode,
                    PortfolioId = string.IsNullOrEmpty(r.PortfolioId) ? long.Parse(PortfolioId) : long.Parse(r.PortfolioId),
                    Symbol = r.InstrumentCode,
                    Price = r.Price,
                    TransactionType = r.TransactionType,
                    TransactionDate = r.TradeDate,
                    Comments = r.Comments,
                    BrokerageCurrencyCode = r.BrokerageCurrencyCode
                });
            }

            return tradeList;
        }

    }
}
