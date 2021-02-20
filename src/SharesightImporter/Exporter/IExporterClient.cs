using System.Collections.Generic;
using System.Threading.Tasks;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Exporter
{
    public interface IExporterClient
    {
        string PortfolioId { get; }
        Task<List<TradePost>> GetTrades();
        int Order { get; }
    }
}