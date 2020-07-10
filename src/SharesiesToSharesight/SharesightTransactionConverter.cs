using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesightImporter.SharesiesClient;
using SharesightImporter.SharesiesClient.Models;
using SharesightImporter.SharesightClient;
using SharesightImporter.SharesightClient.Models;

namespace SharesightImporter
{
    public class SharesightTransactionConverter
    {
        private readonly ILogger<Worker> _logger;
        private readonly Configuration.Configuration _configuration;

        public SharesightTransactionConverter(ILogger<Worker> logger, Configuration.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Dictionary<Guid, string> Symbols { get; set; } = null!;

        public List<TradePost> Convert(Transaction transaction)
        {
            if (transaction.BuyOrder == null && transaction.SellOrder == null)
            {
                _logger.LogInformation("{0} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }

            var trades = new List<TradePost>();
            var order = (transaction.BuyOrder ?? transaction.SellOrder);
            if (order.Trades.Any())
            {
                trades.AddRange(order.Trades.Select(trade => new TradePost
                {
                    Quantity = double.Parse(trade.Volume),
                    Price = double.Parse(trade.SharePrice),
                    TransactionDate = DateTimeOffset.FromUnixTimeMilliseconds(trade.TradeDatetime.Quantum),
                    Brokerage = double.Parse(trade.CorporateFee),
                    Market = order.Mechanism.ToUpper(),
                    PortfolioId = int.Parse(_configuration.SharesightClient.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Symbols[transaction.FundId].ToUpper(),
                    TransactionType = order.Type.ToUpper(),
                    UniqueIdentifier = trade.ContractNoteNumber,
                }));
            }
            else
            {
                trades.Add(new TradePost
                {
                    Quantity = double.Parse(order.OrderShares),
                    Price = double.Parse(order.OrderUnitPrice),
                    TransactionDate = DateTimeOffset.FromUnixTimeMilliseconds(transaction.Timestamp.Quantum),
                    Brokerage = 0.00,
                    Market = "NZX",
                    PortfolioId = int.Parse(_configuration.SharesightClient.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Symbols[transaction.FundId].ToUpper(),
                    TransactionType = order.Type.ToUpper(),
                    UniqueIdentifier = transaction.TransactionId.ToString(),
                });
            }

            return trades;
        }
    }
}