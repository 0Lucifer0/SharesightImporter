﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtherscanApi.Net.Interfaces;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Exporter.EthereumExporter
{
    // Disclaimer this exporter does not know about price. It just use gas fee as price in either brokerage (BUY) or added to quantity (Sell)
    public class EthereumExporterClient : IExporterClient
    {
        public int Order => 3;
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
                    var brokerage = double.Parse(transaction.gasUsed) * double.Parse(transaction.gasPrice) /
                                    1000000000000000000;
                    var quantity = (double) transaction.Value;
                    if (isSell)
                    {
                        quantity = (double)transaction.Value + brokerage;
                        brokerage = 0;
                    }
                    transactions.Add(new TradePost
                    {
                        Quantity = quantity,
                        TransactionDate = transaction.TimeStamp,
                        Market = "FX",
                        PortfolioId = int.Parse(_configuration.PortfolioId),
                        Symbol = "ETH",
                        BrokerageCurrencyCode = "ETH",
                        TransactionType = isSell ? "SELL" : "BUY",
                        UniqueIdentifier = transaction.TxId,
                        Brokerage = brokerage
                    });
                }
            }

            return Task.FromResult(transactions);
        }
    }
}
