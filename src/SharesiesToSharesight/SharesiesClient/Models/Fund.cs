using System;
using System.Text.Json.Serialization;

namespace SharesiesToSharesight.SharesiesClient.Models
{
    public class Fund
    {
        [JsonPropertyName("annualised_return")]
        public string AnnualisedReturn { get; set; } = null!;

        [JsonPropertyName("asset_manager")]
        public string? AssetManager { get; set; }

        [JsonPropertyName("asset_manager_short")]
        public string? AssetManagerShort { get; set; }

        [JsonPropertyName("asset_manager_url")]
        public Uri AssetManagerUrl { get; set; } = null!;

        [JsonPropertyName("categories")]
        public string[] Categories { get; set; } = null!;

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("comparison_prices")]
        public ComparisonPrices ComparisonPrices { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("dividends")]
        public string Dividends { get; set; } = null!;

        [JsonPropertyName("dominant_colour")]
        public string? DominantColour { get; set; }

        [JsonPropertyName("exchange_listing_date")]
        public DateTimeOffset? ExchangeListingDate { get; set; }

        [JsonPropertyName("fund_added_to_sharesies")]
        public FundAddedToSharesies FundAddedToSharesies { get; set; } = null!;

        [JsonPropertyName("fund_image_url")]
        public string FundImageUrl { get; set; } = null!;

        [JsonPropertyName("fund_type")]
        public string FundType { get; set; } = null!;

        [JsonPropertyName("gross_dividend_yield")]
        public string GrossDividendYield { get; set; } = null!;

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("kids_recommended")]
        public bool KidsRecommended { get; set; }

        [JsonPropertyName("locations")]
        public string[] Locations { get; set; } = null!;

        [JsonPropertyName("management_fee")]
        public string ManagementFee { get; set; } = null!;

        [JsonPropertyName("market_last_check")]
        public FundAddedToSharesies MarketLastCheck { get; set; } = null!;

        [JsonPropertyName("market_price")]
        public string MarketPrice { get; set; } = null!;

        [JsonPropertyName("market_state")]
        public string MarketState { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("pds_drive_id")]
        public string PdsDriveId { get; set; } = null!;

        [JsonPropertyName("responsible")]
        public bool Responsible { get; set; }

        [JsonPropertyName("risk_rating")]
        public long RiskRating { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = null!;

        [JsonPropertyName("fixed_fee_spread")]
        public string? FixedFeeSpread { get; set; }
    }
}