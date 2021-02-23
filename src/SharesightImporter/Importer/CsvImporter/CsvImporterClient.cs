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
using SharesightImporter.Exporter.CsvExporter;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Importer.CsvImporter
{
    public class CsvImporterClient : IImporterClient
    {
        private readonly ILogger<CsvImporterClient> _logger;
        private readonly CsvImporterConfiguration _configuration;
        private readonly ILogger<CsvExporterClient> _exporterLogger;

        public CsvImporterClient(ILogger<CsvImporterClient> logger, CsvImporterConfiguration configuration, ILogger<CsvExporterClient> exporterLogger)
        {
            _logger = logger;
            _configuration = configuration;
            _exporterLogger = exporterLogger;
        }

        public async Task<Trade[]> GetTradeHistoryAsync(string porfolioId)
        {
            var exporter = new CsvExporterClient(_exporterLogger, new CsvExporterConfiguration
            {
                Path = _configuration.Path,
                ExporterType = ExporterType.Csv,
            });
            try
            {
                var result = await exporter.GetTrades(); return result.Select(r => new Trade
                {
                    Quantity = r.Quantity,
                    Market = r.Market,
                    PortfolioId = r.PortfolioId,
                    Symbol = r.Symbol,
                    Price = r.Price,
                    TransactionType = r.TransactionType,
                    TransactionDate = r.TransactionDate,
                    Comments = r.Comments,
                    BrokerageCurrencyCode = r.BrokerageCurrencyCode
                }).Where(o => o.PortfolioId?.ToString() == porfolioId).ToArray();
            }
            catch(FileNotFoundException)
            {
                return Array.Empty<Trade>();
            }
        }

        public async Task<long?> AddTradeAsync(TradePost trade)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            var fileExist = File.Exists(_configuration.Path);
            var addHeader = !fileExist || (File.ReadLines(_configuration.Path).FirstOrDefault()?.Length ?? 0) == 0;
            await using var stream = File.Open(_configuration.Path, FileMode.Append);
            await using var writer = new StreamWriter(stream);
            await using var csv = new CsvWriter(writer, config);
            if (addHeader)
            {
                csv.WriteHeader<CsvTrade>();
                await csv.NextRecordAsync();
            }
            csv.WriteRecord(new CsvTrade()
            {
                TransactionType = trade.TransactionType,
                Price = trade.Price,
                MarketCode = trade.Market,
                TradeDate = trade.TransactionDate.DateTime,
                Quantity = trade.Quantity,
                Comments = trade.Comments,
                BrokerageCurrencyCode = trade.BrokerageCurrencyCode,
                InstrumentCode = trade.Symbol,
                PortfolioId = trade.PortfolioId?.ToString()
            });
            await csv.NextRecordAsync();
            return trade.GetHashCode();
        }
    }
}