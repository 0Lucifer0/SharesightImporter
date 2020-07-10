using System.Text.Json.Serialization;

namespace SharesightImporter.SharesiesClient.Models
{
    public class ComparisonPrices
    {
        [JsonPropertyName("1d")]
        public string The1D { get; set; } = null!;

        [JsonPropertyName("1m")]
        public string The1M { get; set; } = null!;

        [JsonPropertyName("1w")]
        public string The1W { get; set; } = null!;

        [JsonPropertyName("1y")]
        public string? The1Y { get; set; } = null!;

        [JsonPropertyName("3m")]
        public string The3M { get; set; } = null!;

        [JsonPropertyName("5y")]
        public string? The5Y { get; set; }

        [JsonPropertyName("6m")]
        public string The6M { get; set; } = null!;
    }
}