using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.Importer.SharesightImporter.SharesightImporterClient.Models
{
    public class ApiTransaction
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("version")]
        public long Version { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; } = null!;

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}