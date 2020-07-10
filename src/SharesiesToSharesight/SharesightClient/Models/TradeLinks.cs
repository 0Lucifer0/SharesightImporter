using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.SharesightClient.Models
{
    public class TradeLinks
    {
        [JsonPropertyName("portfolio")]
        public Uri Portfolio { get; set; } = null!;
    }
}