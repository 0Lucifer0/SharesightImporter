using System.Collections.Generic;
using System.Threading.Tasks;
using SharesightImporter.SharesightClient.Models;

namespace SharesightImporter.Exporter
{
    public interface IExporterClient
    {
        string PortfolioId { get; }
        Task<List<TradePost>> GetTrades();
        int Order { get; }
    }
}