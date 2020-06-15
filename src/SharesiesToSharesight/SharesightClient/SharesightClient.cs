using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesightClient.Models;

namespace SharesiesToSharesight.SharesightClient
{
    public class SharesightClient : ISharesightClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesightClient> _logger;
        private readonly Uri _uri = new Uri("https://api.sharesight.com");
        private readonly Configuration.Configuration _configuration;
        private Token? _token;
        public SharesightClient(IHttpClientFactory clientFactory, ILogger<SharesightClient> logger, Configuration.Configuration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task LoginAsync()
        {
            if (_token != null &&
                DateTimeOffset.FromUnixTimeSeconds(_token.CreatedAt).UtcDateTime.AddSeconds(_token.ExpiresIn) >
                DateTime.Now.ToUniversalTime())
            {
                _logger.LogTrace("Using sharesight cached token");
                return;
            }
            _logger.LogInformation("Connecting to sharesight...");
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var content = new StringContent(JsonSerializer.Serialize(new
            {
                grant_type = "client_credentials",
                client_id = _configuration.SharesightClient.CliendId,
                client_secret = _configuration.SharesightClient.ClientSecret,
            }, options), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("oauth2/token", content);
            if (result.IsSuccessStatusCode)
            {
                var resultJson = await result.Content.ReadAsStringAsync();
                _token = JsonSerializer.Deserialize<Token>(resultJson, options);
                _logger.LogInformation("Connected to sharesight!");
                return;
            }

            _logger.LogError("Connection to sharesight failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        public async Task<TradeHistory> GetTradeHistoryAsync()
        {
            await LoginAsync();
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token!.AccessToken);
            var content = new Dictionary<string, string>
            {
                {"portfolio_id", _configuration.SharesightClient.PortfolioId},
                //{"start_date ", ""},
                //{"start_date ", ""}
                //{"unique_identifier", ""}
            };

            var result = await httpClient.GetAsync(QueryHelpers.AddQueryString("api/v2/trades.json", content));
            if (result.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var resultJson = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TradeHistory>(resultJson, options);
            }
            _logger.LogError("Retrieving trades from sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }

        public async Task AddTradeAsync(TradePost trade)
        {
            await LoginAsync();
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = _uri;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token!.AccessToken);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            var tradeObj = new { Trade = trade };
            var content = new StringContent(JsonSerializer.Serialize(tradeObj, options), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("api/v2/trades.json", content);
            if (result.IsSuccessStatusCode)
            {
                _logger.LogDebug("Trade added to sharesight! {0} {1} {2} {3}", trade.Symbol, trade.Quantity, trade.Price, trade.TransactionDate.Date);
                return;
            }

            _logger.LogError("Adding new trade to sharesight failed", result.ReasonPhrase);
            throw new ArgumentException();
        }
    }
}