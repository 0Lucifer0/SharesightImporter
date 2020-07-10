using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.SharesightClient.Models
{
    public class TradeHistoryLinks
    {
        [JsonPropertyName("self")]
        public Uri Self { get; set; } = null!;
    }
}