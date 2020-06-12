using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace SharesiesToSharesight.SharesightClient
{
    public class SharesightClient : ISharesightClient
    {
        private IHttpClientFactory _clientFactory;
        private ILogger<SharesightClient> _logger;

        public SharesightClient(IHttpClientFactory clientFactory, ILogger<SharesightClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
    }
}