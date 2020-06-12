using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class Transaction
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("balance")]
        public string Balance { get; set; }

        [JsonPropertyName("buy_order")]
        public BuyOrder BuyOrder { get; set; }

        [JsonPropertyName("csn_transfer_order")]
        public object CsnTransferOrder { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("fund_id")]
        public Guid? FundId { get; set; }

        [JsonPropertyName("fx_order")]
        public object FxOrder { get; set; }

        [JsonPropertyName("line_number")]
        public long LineNumber { get; set; }

        [JsonPropertyName("memo")]
        public string Memo { get; set; }

        [JsonPropertyName("order_id")]
        public Guid? OrderId { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("sell_order")]
        public object SellOrder { get; set; }

        [JsonPropertyName("timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonPropertyName("trade")]
        public object Trade { get; set; }

        [JsonPropertyName("transaction_id")]
        public long TransactionId { get; set; }

        [JsonPropertyName("withdrawal_order")]
        public object WithdrawalOrder { get; set; }
    }
}