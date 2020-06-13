using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class TransactionHistory
    {
        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("transactions")]
        public Transaction[] Transactions { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}