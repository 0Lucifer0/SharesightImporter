using System.Text.Json.Serialization;

namespace SharesightImporter.Exporter.SharesiesExporter.Models
{
    public class Timestamp
    {
        [JsonPropertyName("$quantum")]
        public long Quantum { get; set; }
    }
}