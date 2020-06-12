using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharesiesToSharesight.SharesiesClient
{
    public class SharesiesClient : ISharesiesClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesiesClient> _logger;
        const string Url = "https://app.sharesies.nz/api/";
        private CookieContainer _cookies = new CookieContainer();
        private readonly Configuration.Configuration _configuration;

        public SharesiesClient(IHttpClientFactory clientFactory, ILogger<SharesiesClient> logger, Configuration.Configuration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task LoginAsync()
        {
            var uri = new Uri(Url);
            if (_cookies.Count > 0 && !_cookies.GetCookies(uri).ToList().Any(s => s.Expired))
            {
                _logger.LogInformation("Using sharesies cached session");
                return;
            }
            _cookies = new CookieContainer();
            _logger.LogInformation("Connecting to sharesies...");
            var httpClient = _clientFactory.CreateClient();
            httpClient.BaseAddress = uri;
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
                    _cookies.SetCookies(uri, cookie);
                }
                return;
            }

            _logger.LogError("Connection to sharesies failed", result.ReasonPhrase);
            throw new ArgumentException();
        }
    }
}