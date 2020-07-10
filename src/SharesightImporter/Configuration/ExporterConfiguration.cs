using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class ExporterConfiguration
    {
        [Required]
        public ExporterType ExporterType { get; set; }

        [Required]
        public string PortfolioId { get; set; } = null!;
    }

    public enum ExporterType
    {
        Sharesies,
        Ethereum
    }
}