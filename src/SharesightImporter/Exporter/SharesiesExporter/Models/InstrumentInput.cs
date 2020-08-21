using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SharesightImporter.Exporter.SharesiesExporter.Models
{
    public class InstrumentInput
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("sort")]
        public string Sort { get; set; } = null!;

        [JsonPropertyName("priceChangeTime")]
        public string PriceChangeTime { get; set; } = null!;

        [JsonPropertyName("query")]
        public string Query { get; set; } = null!;

        [JsonPropertyName("instruments")]
        public List<string> Instruments { get; set; } = null!;
    }

}
