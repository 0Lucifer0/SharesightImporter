using System.Text.Json.Serialization;

namespace SharesightImporter.SharesiesClient.Models
{
    public class Trade
    {
        [JsonPropertyName("contract_note_number")]
        public string ContractNoteNumber { get; set; } = null!;

        [JsonPropertyName("corporate_fee")]
        public string CorporateFee { get; set; } = null!;

        [JsonPropertyName("created")]
        public Timestamp Created { get; set; } = null!;

        [JsonPropertyName("gross_value")]
        public string GrossValue { get; set; } = null!;

        [JsonPropertyName("share_price")]
        public string SharePrice { get; set; } = null!;

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("trade_datetime")]
        public Timestamp TradeDatetime { get; set; } = null!;

        [JsonPropertyName("volume")]
        public string Volume { get; set; } = null!;
    }
}