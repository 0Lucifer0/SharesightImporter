using System.Text.Json.Serialization;

namespace SharesightImporter.Exporter.SharesiesExporter.Models
{
    public class ComparisonPrices
    {
        [JsonPropertyName("1d")]
        public object The1D { get; set; }

        [JsonPropertyName("1m")]
        public object The1M { get; set; }

        [JsonPropertyName("1w")]
        public object The1W { get; set; }

        [JsonPropertyName("1y")]
        public object The1Y { get; set; }

        [JsonPropertyName("3m")]
        public object The3M { get; set; }

        [JsonPropertyName("5y")]
        public object The5Y { get; set; }

        [JsonPropertyName("6m")]
        public object The6M { get; set; }
    }
}