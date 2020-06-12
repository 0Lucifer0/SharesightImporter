using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient
{
    public class TransactionHistory
    {
        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }

        [JsonPropertyName("transactions")]
        public Transaction[] Transactions { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}