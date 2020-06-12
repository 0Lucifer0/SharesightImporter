using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharesiesToSharesight.SharesiesClient.Models;

namespace SharesiesToSharesight.SharesiesClient
{
    public class SharesiesClient : ISharesiesClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesiesClient> _logger;
        private readonly Uri _uri = new Uri("https://app.sharesies.nz/api/");
        private CookieContainer _cookies = new CookieContainer();
        private readonly Configuration.Configuration _configuration;
        private string? userId = null;
        public SharesiesClient(IHttpClientFactory clientFactory, ILogger<SharesiesClient> logger, Configuration.Configuration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task LoginAsync()
        {
            if (_cookies.Count > 0 && !_cookies.GetCookies(_uri).ToList().Any(s => s.Expired))
            {
                _logger.LogInformation("Using sharesies cached session");
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
                _configuration.SharesiesClient.Email,
                _configuration.SharesiesClient.Password,
                Remember = true
            }, options), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("identity/login", content);
            if (result.IsSuccessStatusCode)
            {
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
                {"acting_as_id", userId!},
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
    }
}