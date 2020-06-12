using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class BuyOrder
    {
        [JsonPropertyName("autoinvest")]
        public bool Autoinvest { get; set; }

        [JsonPropertyName("cancel_transaction_id")]
        public object CancelTransactionId { get; set; }

        [JsonPropertyName("cancelled_at")]
        public object CancelledAt { get; set; }

        [JsonPropertyName("contribution")]
        public object Contribution { get; set; }

        [JsonPropertyName("create_transaction_id")]
        public long CreateTransactionId { get; set; }

        [JsonPropertyName("created")]
        public Timestamp Created { get; set; }

        [JsonPropertyName("delay_reason")]
        public string DelayReason { get; set; }

        [JsonPropertyName("fulfil_transaction_id")]
        public long? FulfilTransactionId { get; set; }

        [JsonPropertyName("fund_id")]
        public Guid FundId { get; set; }

        [JsonPropertyName("gain")]
        public object Gain { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("mechanism")]
        public string Mechanism { get; set; }

        [JsonPropertyName("net_contribution")]
        public object NetContribution { get; set; }

        [JsonPropertyName("net_gain")]
        public object NetGain { get; set; }

        [JsonPropertyName("net_shares")]
        public object NetShares { get; set; }

        [JsonPropertyName("order_shares")]
        public string OrderShares { get; set; }

        [JsonPropertyName("order_total")]
        public string OrderTotal { get; set; }

        [JsonPropertyName("order_transaction_id")]
        public object OrderTransactionId { get; set; }

        [JsonPropertyName("order_unit_price")]
        public string OrderUnitPrice { get; set; }

        [JsonPropertyName("price_limit")]
        public object PriceLimit { get; set; }

        [JsonPropertyName("rejected_at")]
        public object RejectedAt { get; set; }

        [JsonPropertyName("remaining_nzd_amount")]
        public string RemainingNzdAmount { get; set; }

        [JsonPropertyName("requested_nzd_amount")]
        public string RequestedNzdAmount { get; set; }

        [JsonPropertyName("settled")]
        public Timestamp Settled { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("trades")]
        public Trade[] Trades { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("unit_cost")]
        public object UnitCost { get; set; }

        [JsonPropertyName("will_settle_on")]
        public Timestamp WillSettleOn { get; set; }
    }
}