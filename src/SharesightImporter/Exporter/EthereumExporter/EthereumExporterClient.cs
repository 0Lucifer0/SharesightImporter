using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EtherscanApi.Net.Interfaces;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Exporter.SharesiesExporter;
using SharesightImporter.SharesightClient.Models;

namespace SharesightImporter.Exporter.EthereumExporter
{
    public class EthereumExporterClient : IExporterClient
    {
        public string PortfolioId => _configuration.PortfolioId;
        private readonly ILogger<EthereumExporterClient> _logger;
        private readonly EthereumExporterConfiguration _configuration;
        private readonly EtherScanClient _etherScanClient;


        public EthereumExporterClient(ILogger<EthereumExporterClient> logger, EthereumExporterConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _etherScanClient = new EtherScanClient(configuration.EtherscanApiKey);
        }


        public Task<List<TradePost>> GetTrades()
        {
            var transactions = new List<TradePost>();
            foreach (var address in _configuration.Addresses)
            {
                foreach (var transaction in _etherScanClient.GetTransactions(address, sort: "desc").Result)
                {
                    var isSell = transaction.ToId.ToLowerInvariant() != address.ToLowerInvariant();
                    if (transaction.Value == 0)
                    {
                        _logger.LogInformation("{0} transactions not supported because value is 0", transaction.TxId);
                        continue;
                    }
                    transactions.Add(new TradePost
                    {
                        Quantity = (double)transaction.Value,
                        TransactionDate = transaction.TimeStamp,
                        Market = "FX",
                        PortfolioId = int.Parse(_configuration.PortfolioId),
                        Symbol = "ETH",
                        BrokerageCurrencyCode = "ETH",
                        TransactionType = isSell ? "SELL" : "BUY",
                        UniqueIdentifier = transaction.TxId,
                        Brokerage = double.Parse(transaction.gasUsed) * double.Parse(transaction.gasPrice) / 1000000000000000000
                    });
                }
            }
            return Task.FromResult(transactions);
        }
    }
}
