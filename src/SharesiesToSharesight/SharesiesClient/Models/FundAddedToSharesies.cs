using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class FundAddedToSharesies
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}