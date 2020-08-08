using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Exporter.SharesiesExporter.Models;
using SharesightImporter.SharesightClient.Models;

namespace SharesightImporter.Exporter.SharesiesExporter
{
    public class SharesiesExporterClient : ISharesiesExporterClient
    {
        public string PortfolioId => _configuration.PortfolioId;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesiesExporterClient> _logger;
        private readonly Uri _uri = new Uri("https://app.sharesies.nz/api/");
        private CookieContainer _cookies = new CookieContainer();
        private readonly SharesiesExporterConfiguration _configuration;
        private string? _userId = null;


        public SharesiesExporterClient(IHttpClientFactory clientFactory, ILogger<SharesiesExporterClient> logger, SharesiesExporterConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task LoginAsync()
        {
            if (_cookies.Count > 0 && !_cookies.GetCookies(_uri).ToList().Any(s => s.Expired))
            {
                _logger.LogTrace("Using sharesies cached session");
                return;
            }
            _cookies = new CookieContainer();
            _logger.LogInformation("Connecting to sharesies...");
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                _configuration.Email,
                _configuration.Password,
                Remember = true
            }, options), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("identity/login", content);
            if (result.IsSuccessStatusCode)
            {
                var user = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(
                    await result.Content.ReadAsStringAsync())["user"].ToString();
                _userId = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(user)["id"].ToString();
                _logger.LogInformation("Connected to sharesies!");
                foreach (var cookie in result.Headers.First(s => s.Key.Equals("Set-Cookie", StringComparison.CurrentCultureIgnoreCase)).Value)
                {
                    _cookies.SetCookies(_uri, cookie);
                }
                return;
            }

            _logger.LogError("Connection to sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        public Task<TransactionHistory> GetPaymentHistoryAsync() => GetPaymentHistoryAsync(null);

        public async Task<TransactionHistory> GetPaymentHistoryAsync(long? beforeId)
        {
            await LoginAsync();
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            httpClient.DefaultRequestHeaders.Add("Cookie", _cookies.GetCookieHeader(_uri));
            var content = new Dictionary<string, string>
            {
                {"acting_as_id", _userId!},
                {"limit", "50"}
            };
            if (beforeId != null)
            {
                content.Add("before", beforeId.ToString()!);
            }

            var result = await httpClient.GetAsync(QueryHelpers.AddQueryString("accounting/transaction-history", content));
            if (result.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var resultJson = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TransactionHistory>(resultJson, options);
            }
            _logger.LogError("Retrieving transactions from sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        public async Task<Dictionary<Guid, string>> GetSymbolsAsync()
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            var result = await httpClient.GetAsync("fund/list");
            if (result.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var resultJson = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FundList>(resultJson, options).Funds.ToDictionary(s => s.Id, s => s.Code);
            }
            _logger.LogError("Retrieving funds from sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        private List<TradePost> Convert(Transaction transaction)
        {
            if (transaction.BuyOrder == null && transaction.SellOrder == null)
            {
                _logger.LogInformation("{0} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }

            var trades = new List<TradePost>();
            var order = (transaction.BuyOrder ?? transaction.SellOrder);
            if (order.State == "cancelled" || order.State == "pending")
            {
                _logger.LogInformation($"{order.State} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }
            if (order.Trades.Any())
            {
                trades.AddRange(order.Trades.Select(trade => new TradePost
                {
                    Quantity = double.Parse(trade.Volume),
                    Price = double.Parse(trade.SharePrice),
                    TransactionDate = DateTimeOffset.FromUnixTimeMilliseconds(trade.TradeDatetime.Quantum),
                    Brokerage = double.Parse(trade.CorporateFee),
                    Market = order.Mechanism.ToUpper(),
                    PortfolioId = int.Parse(_configuration.PortfolioId),
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
                    PortfolioId = int.Parse(_configuration.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Symbols[transaction.FundId].ToUpper(),
                    TransactionType = order.Type.ToUpper(),
                    UniqueIdentifier = transaction.TransactionId.ToString(),
                });
            }

            return trades;
        }

        private Dictionary<Guid, string> Symbols { get; set; }

        public async Task<List<TradePost>> GetTrades()
        {
            var result = new List<TradePost>();
            Symbols = await GetSymbolsAsync();
            var history = await GetPaymentHistoryAsync();

            for (var i = 0; i < history.Transactions.Length; i++)
            {
                result.AddRange(Convert(history.Transactions[i]));
                if (i != history.Transactions.Length - 1 || !history.HasMore)
                {
                    continue;
                }

                history = await GetPaymentHistoryAsync(history.Transactions[i].TransactionId);
                i = -1;
            }

            return result;
        }
    }
}