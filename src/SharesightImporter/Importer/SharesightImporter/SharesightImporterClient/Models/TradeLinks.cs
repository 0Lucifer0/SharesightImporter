using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models
{
    public class TradeLinks
    {
        [JsonPropertyName("portfolio")]
        public Uri Portfolio { get; set; } = null!;
    }
}