using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient
{
    public class Timestamp
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}