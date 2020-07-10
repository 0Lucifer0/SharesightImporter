using System;
using System.Text.Json.Serialization;

namespace SharesightImporter.Exporter.SharesiesExporter.Models
{
    public class Transaction
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; } = null!;

        [JsonPropertyName("balance")]
        public string Balance { get; set; } = null!;

        [JsonPropertyName("buy_order")]
        public Order BuyOrder { get; set; } = null!;

        [JsonPropertyName("csn_transfer_order")]
        public object CsnTransferOrder { get; set; } = null!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("fund_id")]
        public Guid FundId { get; set; }

        [JsonPropertyName("fx_order")]
        public object FxOrder { get; set; } = null!;

        [JsonPropertyName("line_number")]
        public long LineNumber { get; set; }

        [JsonPropertyName("memo")]
        public string Memo { get; set; } = null!;

        [JsonPropertyName("order_id")]
        public Guid? OrderId { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = null!;

        [JsonPropertyName("sell_order")]
        public Order SellOrder { get; set; } = null!;

        [JsonPropertyName("timestamp")]
        public Timestamp Timestamp { get; set; } = null!;

        [JsonPropertyName("trade")]
        public object Trade { get; set; } = null!;

        [JsonPropertyName("transaction_id")]
        public long TransactionId { get; set; }

        [JsonPropertyName("withdrawal_order")]
        public object WithdrawalOrder { get; set; } = null!;
    }
}