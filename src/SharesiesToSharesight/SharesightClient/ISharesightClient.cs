using SharesiesToSharesight.SharesightClient.Models;
using System.Threading.Tasks;

namespace SharesiesToSharesight.SharesightClient
{
    public interface ISharesightClient
    {
        public Task LoginAsync();
        public Task<bool> GetMatchingTransactionAsync(TradePost trade);
        public Task AddTradeAsync(TradePost trade);
    }
}