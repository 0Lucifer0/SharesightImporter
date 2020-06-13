using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesiesClient;
using SharesiesToSharesight.SharesiesClient.Models;
using SharesiesToSharesight.SharesightClient;
using SharesiesToSharesight.SharesightClient.Models;

namespace SharesiesToSharesight
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
            if (transaction.BuyOrder == null)
            {
                _logger.LogInformation("{0} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }

            var trades = new List<TradePost>();
            if (transaction.BuyOrder.Trades.Any())
            {
                trades.AddRange(transaction.BuyOrder.Trades.Select(trade => new TradePost
                {
                    Quantity = double.Parse(trade.Volume),
                    Price = double.Parse(trade.SharePrice),
                    TransactionDate = DateTimeOffset.FromUnixTimeMilliseconds(trade.TradeDatetime.Quantum),
                    Brokerage = double.Parse(trade.CorporateFee),
                    Market = transaction.BuyOrder.Mechanism.ToUpper(),
                    PortfolioId = int.Parse(_configuration.SharesightClient.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Symbols[transaction.FundId].ToUpper(),
                    TransactionType = transaction.BuyOrder.Type.ToUpper(),
                    UniqueIdentifier = trade.ContractNoteNumber,
                }));
            }
            else
            {
                trades.Add(new TradePost
                {
                    Quantity = double.Parse(transaction.BuyOrder.OrderShares),
                    Price = double.Parse(transaction.BuyOrder.OrderUnitPrice),
                    TransactionDate = DateTimeOffset.FromUnixTimeMilliseconds(transaction.Timestamp.Quantum),
                    Brokerage = 0.00,
                    Market = "NZX",
                    PortfolioId = int.Parse(_configuration.SharesightClient.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Symbols[transaction.FundId].ToUpper(),
                    TransactionType = transaction.BuyOrder.Type.ToUpper(),
                    UniqueIdentifier = transaction.TransactionId.ToString(),
                });
            }

            return trades;
        }
    }
}