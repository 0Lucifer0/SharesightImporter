using System.Threading.Tasks;
using SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models;

namespace SharesightImporter.Importer.SharesightImporter.SharesightImporterClient
{
    public interface ISharesightImporterClient : IImporterClient
    {
        Task RemoveTradeAsync(long identifier, string portfolioId);
    }
}