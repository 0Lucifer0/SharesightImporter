using System.ComponentModel.DataAnnotations;

namespace SharesiesToSharesight.Configuration
{
    public class SharesightClientConfiguration
    {
        [Required]
        public string CliendId { get; set; } = null!;

        [Required]
        public string ClientSecret { get; set; } = null!;

        [Required]
        public string PortfolioId { get; set; } = null!;
    }
}