using System.Text.Json.Serialization;

namespace SharesightImporter.SharesiesClient.Models
{
    public class FundAddedToSharesies
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}