using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesightClient.Models
{
    public class TradeLinks
    {
        [JsonPropertyName("portfolio")]
        public Uri Portfolio { get; set; } = null!;
    }
}