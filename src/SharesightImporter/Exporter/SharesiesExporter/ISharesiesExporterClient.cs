using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharesightImporter.Exporter.SharesiesExporter.Models;

namespace SharesightImporter.Exporter.SharesiesExporter
{
    public interface ISharesiesExporterClient : IExporterClient
    {
        public Task LoginAsync();
        public Task<TransactionHistory> GetPaymentHistoryAsync();
        public Task<TransactionHistory> GetPaymentHistoryAsync(long? beforeId);
        public Task<Dictionary<Guid, string>> GetSymbolsAsync();
    }
}