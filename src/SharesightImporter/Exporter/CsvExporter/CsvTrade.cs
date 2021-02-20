using System;
using System.ComponentModel.DataAnnotations;
using SharesightImporter.Exporter.SharesiesExporter.Models;

namespace SharesightImporter.Exporter.CsvExporter
{
    public class CsvTrade
    {
        public string PortfolioId { get; set; } = null!;

        [Required]
        public DateTime TradeDate { get; set; }

        [Required]
        public string InstrumentCode { get; set; } = null!;

        [Required]
        public string MarketCode { get; set; } = null!;

        public double? Quantity { get; set; }

        public double? Price { get; set; }

        [Required]
        public string TransactionType { get; set; } = null!;

        public string? Comments { get; set; }

        public string? BrokerageCurrencyCode { get; set; }
    }
}