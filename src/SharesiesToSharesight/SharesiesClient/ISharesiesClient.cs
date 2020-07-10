using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharesightImporter.SharesiesClient.Models;

namespace SharesightImporter.SharesiesClient
{
    public interface ISharesiesClient
    {
        public Task LoginAsync();
        public Task<TransactionHistory> GetPaymentHistoryAsync();
        public Task<TransactionHistory> GetPaymentHistoryAsync(long? beforeId);
        public Task<Dictionary<Guid, string>> GetSymbolsAsync();
    }
}