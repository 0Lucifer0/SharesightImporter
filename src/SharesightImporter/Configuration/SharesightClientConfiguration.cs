using System.ComponentModel.DataAnnotations;

namespace SharesightImporter.Configuration
{
    public class SharesightClientConfiguration
    {
        [Required]
        public string CliendId { get; set; } = null!;

        [Required]
        public string ClientSecret { get; set; } = null!;
    }
}