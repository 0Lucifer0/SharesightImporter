using System;
using Newtonsoft.Json;

namespace SharesightImporter.Exporter.SharesiesExporter.Models
{
    public class InstrumentResult
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }

        [JsonProperty("numberOfPages")]
        public long NumberOfPages { get; set; }

        [JsonProperty("instruments")] 
        public Instrument[] Instruments { get; set; } = null!;
    }

    public class Instrument
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("urlSlug")]
        public string UrlSlug { get; set; } = null!;

        [JsonProperty("instrumentType")]
        public string InstrumentType { get; set; } = null!;

        [JsonProperty("symbol")]
        public string Symbol { get; set; } = null!;

        [JsonProperty("kidsRecommended")]
        public bool KidsRecommended { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("categories")]
        public string[] Categories { get; set; } = null!;

        [JsonProperty("logoIdentifier")]
        public Guid LogoIdentifier { get; set; }

        [JsonProperty("logos")]
        public Logos Logos { get; set; } = null!;

        [JsonProperty("riskRating")]
        public long RiskRating { get; set; }

        [JsonProperty("comparisonPrices")]
        public ComparisonPrices ComparisonPrices { get; set; } = null!;

        [JsonProperty("marketPrice")]
        public string MarketPrice { get; set; } = null!;

        [JsonProperty("marketLastCheck")]
        public DateTimeOffset MarketLastCheck { get; set; }

        [JsonProperty("tradingStatus")]
        public string TradingStatus { get; set; } = null!;

        [JsonProperty("peRatio")]
        public string PeRatio { get; set; } = null!;

        [JsonProperty("marketCap")]
        public long MarketCap { get; set; }

        [JsonProperty("websiteUrl")]
        public string WebsiteUrl { get; set; } = null!;

        [JsonProperty("exchange")]
        public string Exchange { get; set; } = null!;

        [JsonProperty("legacyImageUrl")]
        public string LegacyImageUrl { get; set; } = null!;

        [JsonProperty("dominantColour")]
        public string DominantColour { get; set; } = null!;

        [JsonProperty("pdsDriveId")]
        public string PdsDriveId { get; set; } = null!;

        [JsonProperty("assetManager")]
        public Guid? AssetManager { get; set; }

        [JsonProperty("fixedFeeSpread")]
        public object FixedFeeSpread { get; set; } = null!;

        [JsonProperty("managementFeePercent")]
        public string ManagementFeePercent { get; set; } = null!;

        [JsonProperty("grossDividendYieldPercent")]
        public string GrossDividendYieldPercent { get; set; } = null!;

        [JsonProperty("annualisedReturnPercent")]
        public string AnnualisedReturnPercent { get; set; } = null!;

        [JsonProperty("ceo")]
        public string Ceo { get; set; } = null!;

        [JsonProperty("employees")]
        public long Employees { get; set; }
    }

    public class Logos
    {
        [JsonProperty("wide")]
        public string Wide { get; set; } = null!;

        [JsonProperty("thumb")]
        public string Thumb { get; set; } = null!;

        [JsonProperty("micro")]
        public string Micro { get; set; } = null!;
    }
}
