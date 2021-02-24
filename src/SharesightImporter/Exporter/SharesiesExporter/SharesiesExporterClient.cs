using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharesightImporter.Configuration;
using SharesightImporter.Exporter.SharesiesExporter.Models;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Exporter.SharesiesExporter
{
    public class SharesiesExporterClient : ISharesiesExporterClient
    {
        public int Order => 2;
        public string PortfolioId => _configuration.PortfolioId;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesiesExporterClient> _logger;
        private readonly Uri _uri = new Uri("https://app.sharesies.nz/api/");
        private CookieContainer _cookies = new CookieContainer();
        private readonly SharesiesExporterConfiguration _configuration;
        private string? _userId = null;
        private string? _bearer;
        private DateTime _bearerRefresh = default!;


        public SharesiesExporterClient(IHttpClientFactory clientFactory, ILogger<SharesiesExporterClient> logger, SharesiesExporterConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task LoginAsync()
        {
            _logger.LogInformation("Connecting to sharesies...");
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;

            if (_cookies.Count > 0 && !_cookies.GetCookies(_uri).ToList().Any(s => s.Expired) && (DateTime.Now - _bearerRefresh).TotalMinutes < 10)
            {
                _logger.LogTrace("Using sharesies cached session");
                return;
            }


            _cookies = new CookieContainer();
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
                var resultdic = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(
                    await result.Content.ReadAsStringAsync());
                var user = resultdic["user"].ToString();
                _bearer = resultdic["distill_token"].ToString();
                _bearerRefresh = DateTime.Now;
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

        public async Task<Dictionary<Guid, Instrument>> GetInstrumentsAsync(List<Guid> instrumentsId)
        {
            await LoginAsync();
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearer);
            httpClient.BaseAddress = new Uri("https://data.sharesies.nz/api/v1/");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            var content = new StringContent(JsonSerializer.Serialize(new InstrumentInput
            {
                Page = 1,
                Instruments = instrumentsId.Select(s => s.ToString()).ToList(),
                PriceChangeTime = "1y",
                Query = "",
                Sort = "name"
            }, options), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("instruments", content);
            if (result.IsSuccessStatusCode)
            {
                var resultJson = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InstrumentResult>(resultJson, options).Instruments.ToDictionary(s => s.Id, s => s);
            }
            _logger.LogError("Retrieving funds from sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        private List<TradePost> Convert(Transaction transaction)
        {
            var orderType = transaction switch
            {
                var x when x.BuyOrder != null => "Buy",
                var x when x.SellOrder != null => "Sell",
                var x when x.CsnTransferOrder != null => "Csn",
                var x when x.FxOrder != null => "Fx",
                var x when x.WithdrawalOrder != null => "Withdrawal",
                _ => null
            };

            if (orderType == null)
            {
                _logger.LogInformation("{0} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }

            var trades = new List<TradePost>();
            var order = (transaction.BuyOrder ?? transaction.SellOrder ?? transaction.FxOrder ?? transaction.CsnTransferOrder ?? transaction.WithdrawalOrder);
            if (order.State != "fulfilled")
            {
                _logger.LogInformation($"{order.State} transactions not supported", transaction.Reason);
                return new List<TradePost>();
            }

            var transactionType = orderType switch
            {
                "Buy" => "BUY",
                "Sell" => "SELL",
                "Csn" => "BUY",
                _ => null
            };

            if (transactionType == null)
            {
                _logger.LogInformation("{0} orders are not supported", orderType);
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
                    Market = Instruments[transaction.FundId].Exchange,
                    PortfolioId = int.Parse(_configuration.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Instruments[transaction.FundId].Symbol.ToUpper(),
                    TransactionType = transactionType,
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
                    Market = Instruments[transaction.FundId].Exchange,
                    PortfolioId = int.Parse(_configuration.PortfolioId),
                    BrokerageCurrencyCode = transaction.Currency.ToUpper(),
                    Symbol = Instruments[transaction.FundId].Symbol.ToUpper(),
                    TransactionType = transactionType,
                    UniqueIdentifier = transaction.TransactionId.ToString(),
                });
            }

            return trades;
        }

        private Dictionary<Guid, Instrument> Instruments { get; set; } = default!;

        public async Task<List<TradePost>> GetTrades()
        {
            var result = new List<TradePost>();
            var history = await GetPaymentHistoryAsync();
            Instruments = await GetInstrumentsAsync(history.Transactions.Select(s => s.FundId).ToList());
            for (var i = 0; i < history.Transactions.Length; i++)
            {
                result.AddRange(Convert(history.Transactions[i]));
                if (i != history.Transactions.Length - 1 || !history.HasMore)
                {
                    continue;
                }

                history = await GetPaymentHistoryAsync(history.Transactions[i].TransactionId);
                var missing = history.Transactions.Select(s => s.FundId).Except(Instruments.Keys).Where(s => s != Guid.Empty).ToList();
                if (missing.Any())
                {
                    var missinginstruments = await GetInstrumentsAsync(missing.ToList());
                    foreach (var instr in missinginstruments)
                    {
                        Instruments.Add(instr.Key, instr.Value);
                    }
                }
                i = -1;
            }

            return result;
        }
    }
}