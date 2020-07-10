using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharesightImporter.SharesightClient.Models
{
    public class Trade : TradePost
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("paid_on")]
        public object PaidOn { get; set; } = null!;

        [JsonPropertyName("company_event_id")]
        public object CompanyEventId { get; set; } = null!;

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("attachment_id")]
        public long? AttachmentId { get; set; }

        [JsonPropertyName("instrument_id")]
        public long InstrumentId { get; set; }

        [JsonPropertyName("links")]
        public TradeLinks Links { get; set; } = null!;
    }

    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTimeOffset.ParseExact(reader.GetString(),
                "yyyy-MM-dd", CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToString(
                "yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
    public class TradePost
    {
        [JsonPropertyName("unique_identifier")]
        public string? UniqueIdentifier { get; set; }

        [JsonPropertyName("transaction_type")]
        public string? TransactionType { get; set; }

        [JsonPropertyName("transaction_date")]
        public DateTimeOffset TransactionDate { get; set; }

        [JsonPropertyName("portfolio_id")]
        public long? PortfolioId { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("market")]
        public string? Market { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("exchange_rate")]
        public double? ExchangeRate { get; set; }

        [JsonPropertyName("brokerage")]
        public double? Brokerage { get; set; }

        [JsonPropertyName("brokerage_currency_code")]
        public string? BrokerageCurrencyCode { get; set; }

        [JsonPropertyName("comments")]
        public string? Comments { get; set; }

        [JsonPropertyName("attachment")]
        public string? Attachment { get; set; } = null!;

        [JsonPropertyName("attachment_filename")]
        public string? AttachmentFilename { get; set; }

        [JsonPropertyName("holding_id")]
        public long? HoldingId { get; set; }
    }
}