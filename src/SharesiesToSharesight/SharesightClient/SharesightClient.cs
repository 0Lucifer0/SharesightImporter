using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace SharesiesToSharesight.SharesightClient
{
    public class SharesightClient : ISharesightClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SharesightClient> _logger;

        public SharesightClient(IHttpClientFactory clientFactory, ILogger<SharesightClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
    }
}