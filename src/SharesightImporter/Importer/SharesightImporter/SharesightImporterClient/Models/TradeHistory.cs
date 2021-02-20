using System.Text.Json.Serialization;

namespace SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models
{

    public class TradeHistory
    {
        [JsonPropertyName("trades")]
        public Trade[] Trades { get; set; } = null!;

        [JsonPropertyName("api_transaction")]
        public ApiTransaction ApiTransaction { get; set; } = null!;

        [JsonPropertyName("links")]
        public TradeHistoryLinks Links { get; set; } = null!;
    }
}
