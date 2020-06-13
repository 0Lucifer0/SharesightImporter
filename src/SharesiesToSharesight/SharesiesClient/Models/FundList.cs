using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class FundList
    {
        [JsonPropertyName("categories")]
        public string[] Categories { get; set; } = null!;

        [JsonPropertyName("funds")]
        public Fund[] Funds { get; set; } = null!;

        [JsonPropertyName("locations")]
        public string[] Locations { get; set; } = null!;

        [JsonPropertyName("most_popular_funds")]
        public Guid[] MostPopularFunds { get; set; } = null!;

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }
}