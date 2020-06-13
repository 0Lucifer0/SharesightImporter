using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesightClient.Models
{

    public class TradeHistory
    {
        [JsonPropertyName("trades")]
        public Trade[] Trades { get; set; } = null!;

        [JsonPropertyName("api_transaction")]
        public ApiTransaction ApiTransaction { get; set; } = null!;

        [JsonPropertyName("links")]
        public TradeHistoryLinks Links { get; set; } = null!;
    }
}
