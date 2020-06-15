using SharesiesToSharesight.SharesightClient.Models;
using System.Threading.Tasks;

namespace SharesiesToSharesight.SharesightClient
{
    public interface ISharesightClient
    {
        public Task LoginAsync();
        public Task<TradeHistory> GetTradeHistoryAsync();
        public Task AddTradeAsync(TradePost trade);
    }
}