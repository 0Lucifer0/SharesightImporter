using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient
{
    public class Trade
    {
        [JsonPropertyName("contract_note_number")]
        public string ContractNoteNumber { get; set; }

        [JsonPropertyName("corporate_fee")]
        public string CorporateFee { get; set; }

        [JsonPropertyName("created")]
        public Timestamp Created { get; set; }

        [JsonPropertyName("gross_value")]
        public string GrossValue { get; set; }

        [JsonPropertyName("share_price")]
        public string SharePrice { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("trade_datetime")]
        public Timestamp TradeDatetime { get; set; }

        [JsonPropertyName("volume")]
        public string Volume { get; set; }
    }
}