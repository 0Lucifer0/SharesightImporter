using SharesightImporter.SharesightClient.Models;
using System.Threading.Tasks;

namespace SharesightImporter.SharesightClient
{
    public interface ISharesightClient
    {
        public Task LoginAsync();
        public Task<TradeHistory> GetTradeHistoryAsync(string portfolioId);
        public Task<long> AddTradeAsync(TradePost trade);
        public Task RemoveTradeAsync(long identifier, string portfolioId);
    }
}