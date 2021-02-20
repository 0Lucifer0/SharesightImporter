using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Importer
{
    public interface IImporterClient
    {
         Task<TradeHistory> GetTradeHistoryAsync(string portfolioId);
         Task<long> AddTradeAsync(TradePost trade);
    }
}
