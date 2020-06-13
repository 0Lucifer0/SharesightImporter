using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesightClient.Models
{
    public class TradeHistoryLinks
    {
        [JsonPropertyName("self")]
        public Uri Self { get; set; } = null!;
    }
}