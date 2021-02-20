using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var values = line.Split(',');
                //Trade Date,Instrument Code,Market Code,Quantity,Price,Transaction Type,Comments (optional), Brokerage Currency Code(optional)
                if (values.Length > 5)
                {
                    var date = values[0];
                    var instrument = values[1];
                    var market = values[2];
                    var qty = values[3];
                    var price = values[4];
                    var type = values[5];
                    var comments = values[6];
                    var brokerageCode = values[7];
                    double priceDouble = 0;
                    double qtyDouble = 0;
                    if ((string.IsNullOrEmpty(qty) || double.TryParse(qty, out qtyDouble))
                        && (string.IsNullOrEmpty(price) || double.TryParse(price, out priceDouble)) &&
                        DateTimeOffset.TryParseExact(date, new[] { "yyyy-MM-dd HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dateDateTimeOffset))
                    {
                        tradeList.Add(new TradePost
                        {
                            Quantity = string.IsNullOrEmpty(qty) ? null : (double?)qtyDouble,
                            Market = market,
                            PortfolioId = long.Parse(PortfolioId),
                            Symbol = instrument,
                            Price = string.IsNullOrEmpty(price) ? null : (double?)priceDouble,
                            TransactionType = type,
                            TransactionDate = dateDateTimeOffset,
                            Comments = string.IsNullOrEmpty(comments) ? null : comments,
                            BrokerageCurrencyCode = brokerageCode
                        });
                    }
                }
            }

            return tradeList;
        }

    }
}
