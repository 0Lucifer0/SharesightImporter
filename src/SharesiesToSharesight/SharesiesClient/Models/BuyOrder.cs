using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class BuyOrder
    {
        [JsonPropertyName("autoinvest")]
        public bool Autoinvest { get; set; }

        [JsonPropertyName("cancel_transaction_id")]
        public object CancelTransactionId { get; set; } = null!;

        [JsonPropertyName("cancelled_at")]
        public object CancelledAt { get; set; } = null!;

        [JsonPropertyName("contribution")]
        public object Contribution { get; set; } = null!;

        [JsonPropertyName("create_transaction_id")]
        public long CreateTransactionId { get; set; }

        [JsonPropertyName("created")]
        public Timestamp Created { get; set; } = null!;

        [JsonPropertyName("delay_reason")]
        public string DelayReason { get; set; } = null!;

        [JsonPropertyName("fulfil_transaction_id")]
        public long? FulfilTransactionId { get; set; }

        [JsonPropertyName("fund_id")]
        public Guid FundId { get; set; }

        [JsonPropertyName("gain")]
        public object Gain { get; set; } = null!;

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("mechanism")]
        public string Mechanism { get; set; } = null!;

        [JsonPropertyName("net_contribution")]
        public object NetContribution { get; set; } = null!;

        [JsonPropertyName("net_gain")]
        public object NetGain { get; set; } = null!;

        [JsonPropertyName("net_shares")]
        public object NetShares { get; set; } = null!;

        [JsonPropertyName("order_shares")]
        public string OrderShares { get; set; } = null!;

        [JsonPropertyName("order_total")]
        public string OrderTotal { get; set; } = null!;

        [JsonPropertyName("order_transaction_id")]
        public object OrderTransactionId { get; set; } = null!;

        [JsonPropertyName("order_unit_price")]
        public string OrderUnitPrice { get; set; } = null!;

        [JsonPropertyName("price_limit")]
        public object PriceLimit { get; set; } = null!;

        [JsonPropertyName("rejected_at")]
        public object RejectedAt { get; set; } = null!;

        [JsonPropertyName("remaining_nzd_amount")]
        public string RemainingNzdAmount { get; set; } = null!;

        [JsonPropertyName("requested_nzd_amount")]
        public string RequestedNzdAmount { get; set; } = null!;

        [JsonPropertyName("settled")]
        public Timestamp Settled { get; set; } = null!;

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("trades")]
        public Trade[] Trades { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("unit_cost")]
        public object UnitCost { get; set; } = null!;

        [JsonPropertyName("will_settle_on")]
        public Timestamp WillSettleOn { get; set; } = null!;
    }
}