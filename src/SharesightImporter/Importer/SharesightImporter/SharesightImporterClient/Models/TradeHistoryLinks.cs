using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models
{
    public class TradeHistoryLinks
    {
        [JsonPropertyName("self")]
        public Uri Self { get; set; } = null!;
    }
}