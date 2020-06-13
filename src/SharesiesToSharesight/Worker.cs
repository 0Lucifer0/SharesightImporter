using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesiesClient;
using SharesiesToSharesight.SharesightClient;
using SharesiesToSharesight.SharesightClient.Models;

namespace SharesiesToSharesight
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISharesightClient _sharesightClient;
        private readonly ISharesiesClient _sharesiesClient;
        private readonly Configuration.Configuration _configuration;

        public Worker(ILogger<Worker> logger, ISharesiesClient sharesiesClient, ISharesightClient sharesightClient, Configuration.Configuration configuration)
        {
            _logger = logger;
            _sharesiesClient = sharesiesClient;
            _sharesightClient = sharesightClient;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var symbols = await _sharesiesClient.GetSymbolsAsync();
                    var history = await _sharesiesClient.GetPaymentHistoryAsync();
                    for (var i = 0; i < history.Transactions.Length; i++)
                    {
                        var transaction = history.Transactions[i];

                        if (transaction.BuyOrder == null)
                        {
                            _logger.LogInformation("{0} transactions not supported", transaction.Reason);
                            continue;
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
                                Symbol = symbols[transaction.FundId].ToUpper(),
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
                                Symbol = symbols[transaction.FundId].ToUpper(),
                                TransactionType = transaction.BuyOrder.Type.ToUpper(),
                                UniqueIdentifier = transaction.TransactionId.ToString(),
                            });
                        }

                        foreach (var trade in trades)
                        {
                            if (await _sharesightClient.GetMatchingTransactionAsync(trade))
                            {
                                continue;
                            }
                            await _sharesightClient.AddTradeAsync(trade);
                        }


                        if (i != history.Transactions.Length - 1 || !history.HasMore)
                        {
                            continue;
                        }

                        history = await _sharesiesClient.GetPaymentHistoryAsync(transaction.TransactionId);
                        i = 0;
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