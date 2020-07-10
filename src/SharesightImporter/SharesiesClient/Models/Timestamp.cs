using System.Text.Json.Serialization;

namespace SharesightImporter.SharesiesClient.Models
{
    public class Timestamp
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}