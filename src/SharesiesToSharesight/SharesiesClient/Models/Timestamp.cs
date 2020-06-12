using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class Timestamp
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}